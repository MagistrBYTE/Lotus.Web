using System.Collections.Concurrent;
using System.Text.Json;

namespace Lotus.Web
{
    /// <inheritdoc/>
    public class SseService : ISseService
    {
        private readonly ILogger<SseService> _logger;
        // Храним connections, где Key - уникальный ID соединения, а Value - объект с данными
        private readonly ConcurrentDictionary<string, SseConnection> _connections = new();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="logger">Логгер</param>
        /// <param name="applicationLifetime">Уведомления о событиях жизненного цикла приложения.</param>
        public SseService(ILogger<SseService> logger, IHostApplicationLifetime applicationLifetime)
        {
            _logger = logger;
            applicationLifetime.ApplicationStopping.Register(OnShutdown);
        }

        /// <inheritdoc/>
        public async Task AddAsync(HttpContext context)
        {
            // Генерируем уникальный ID для ЭТОГО соединения (вкладки)
            var connectionId = Guid.NewGuid().ToString();
            var userId = GetUserId(context); // Получаем ID пользователя, а не ID соединения

            // Создаем токен отмены, связанный с отключением клиента И остановкой приложения
            // (упрощенный вариант, можно использовать LinkedTokenSource)
            var connectionCts = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted);

            var connection = new SseConnection()
            {
                Response = context.Response,
                Cancel = connectionCts,
                UserId = userId // Сохраняем UserId внутри, чтобы знать, кому отправлять
            };

            if (_connections.TryAdd(connectionId, connection))
            {
                _logger.LogInformation("User {UserId} connected with ConnId {ConnectionId}", userId, connectionId);

                try
                {
                    // Инициализация SSE заголовков (должно быть реализовано в расширении или вручную)
                    // context.Response.ContentType = "text/event-stream";
                    // context.Response.Headers.Add("Cache-Control", "no-cache");
                    connection.Response.SSEInitAsync();

                    // Отправляем сообщение о успешном подключении (если нужно)
                    await SendWelcomeMessageAsync(connection, connectionId);

                    // Асинхронно ждем, пока соединение не будет разорвано (клиентом или сервером)
                    // Это предотвратит завершение метода и закрытие запроса
                    await Task.Delay(Timeout.Infinite, connectionCts.Token);
                }
                catch (TaskCanceledException exc)
                {
                    // Нормальное отключение
#pragma warning disable CA1873
                    _logger.LogDebug(exc, "Connection {ConnectionId} closed.", connectionId);
#pragma warning restore CA1873
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc, "Error on connection {ConnectionId}", connectionId);
                }
                finally
                {
                    // В любом случае удаляем соединение из списка
                    RemoveConnection(connectionId);
                }
            }
        }

        /// <inheritdoc/>
        public async Task SendMessageAsync(BaseMessage message)
        {
            var messageJson = JsonSerializer.Serialize(message);
            var removedKeys = new List<string>();

            foreach (var c in _connections)
            {
                try
                {
                    // Исправлено: \n\n вместо \r\r
                    await c.Value.Response.WriteAsync($"data: {messageJson}\n\n", c.Value.Cancel.Token);
                    await c.Value.Response.Body.FlushAsync(c.Value.Cancel.Token);
                }
                catch (Exception ex)
                {
                    // Логируем ошибку только если это не нормальное отключение (OperationCanceledException)
                    if (ex is not OperationCanceledException)
                    {
                        _logger.LogWarning(ex, "Failed to send message to connection {Id}", c.Key);
                    }
                    removedKeys.Add(c.Key);
                }
            }

            // Удаляем мертвые соединения
            foreach (var key in removedKeys)
            {
                RemoveConnection(key);
            }
        }

        /// <inheritdoc/>
        public virtual string GetUserId(HttpContext context)
        {
            return context.GetClaimValueSubOrDefault() ?? Guid.NewGuid().ToString();
        }

        private void RemoveConnection(string id)
        {
            // Исправлено: TryGetValue вместо FirstOrDefault для скорости O(1)
            if (_connections.TryRemove(id, out var connection))
            {
                try
                {
                    connection.Cancel.Cancel(); // Освобождаем ресурсы токена
                }
                catch (ObjectDisposedException) { }
#pragma warning disable CA1873
                _logger.LogInformation("Connection {Id} removed.", id);
#pragma warning restore CA1873
            }
        }

        private async Task SendWelcomeMessageAsync(SseConnection connection, string connectionId)
        {
            try
            {
                var initMessage = JsonSerializer.Serialize(new ConnectionId { ConnectId = connectionId });

                // Исправлено: \n\n
                await connection.Response.WriteAsync($"data: {initMessage}\n\n", connection.Cancel.Token);
                await connection.Response.Body.FlushAsync(connection.Cancel.Token);
            }
            catch (OperationCanceledException)
            {
                // Игнорируем, если клиент отвалился моментально
            }
            catch (Exception exc)
            {
#pragma warning disable CA1873
                _logger.LogError(exc, "Welcome failed: {Message}", exc.Message);
#pragma warning restore CA1873
            }
        }

        private void OnShutdown()
        {
            _logger.LogInformation("Application stopping. Closing SSE connections...");

            // Проходим по всем соединениям и отменяем их токены
            // Это вызовет TaskCanceledException в Task.Delay внутри AddAsync
            foreach (var c in _connections)
            {
                try
                {
                    c.Value.Cancel.Cancel();
                }
                catch (ObjectDisposedException) { }
            }

            // Очистка словаря опциональна, так как AddAsync сам сделает TryRemove в finally,
            // но мы можем подождать завершения всех задач, если бы хранили их Task.
        }
    }
}
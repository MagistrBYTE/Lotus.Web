
namespace Lotus.Web
{
    /// <summary>
    /// Сервис для работы с технологией Server Sent Event.
    /// </summary>
    public interface ISseService
    {
        /// <summary>
        /// Добавить контекст запроса к последующей обработки.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <returns>Задача.</returns>
        Task AddAsync(HttpContext context);

        /// <summary>
        /// Послать сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Задача.</returns>
        Task SendMessageAsync(BaseMessage message);

        /// <summary>
        /// Получение уникального идентификатора соединения.
        /// </summary>
        /// <param name="context">Контекст запроса.</param>
        /// <returns>Уникальный идентификатор соединения.</returns>
        string GetUserId(HttpContext context);
    }
}
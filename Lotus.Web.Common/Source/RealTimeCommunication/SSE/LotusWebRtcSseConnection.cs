namespace Lotus.Web
{
    /// <summary>
    /// Модель для поддержки соединения по технологии Server Sent Event.
    /// </summary>
    public record SseConnection
    {
        /// <summary>
        /// Ответ.
        /// </summary>
        public HttpResponse Response { get; init; } = null!;

        /// <summary>
        /// Токен отмены.
        /// </summary>
        public CancellationTokenSource Cancel { get; init; } = null!;

        /// <summary>
        /// ID пользователя.
        /// </summary>
        public string UserId { get; set; }
    }
}
using System.Net.Http.Headers;
using System.Text.Json;

using Lotus.Core;

using Microsoft.Net.Http.Headers;

using UAParser;

namespace Lotus.Web
{
    /** \addtogroup WebCommonExtension
	*@{*/
    /// <summary>
    /// Статический класс реализующий методы расширения для работы с контекстом запроса.
    /// </summary>
    public static class XHttpContextExtension
    {
        /// <summary>
        /// Получение токена доступа через контекст запроса.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <returns>Токен доступа.</returns>
        public static string GetAccessToken(this HttpContext httpContext)
        {
            var authorization = httpContext.Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return headerValue.Parameter ?? string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Получение клайма Sub или значение по умолчанию.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <returns>Клайм Sub или значение по умолчанию.</returns>
        public static string? GetClaimValueSubOrDefault(this HttpContext httpContext)
        {
            return httpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        }

        /// <summary>
        /// Получение значение указанного заголовка из запроса.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="headerKey">Имя заголовка.</param>
        /// <returns>Значение указанного заголовка или null</returns>
        public static string? GetRequestHeaderValue(this HttpContext httpContext, string headerKey)
        {
            if (httpContext.Request.Headers.TryGetValue(headerKey, out var values))
            {
                return values.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Получение значение указанного заголовка из запроса.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <param name="headerKey">Имя заголовка.</param>
        /// <param name="defaultValue">Значение по умолчанию.</param>
        /// <returns>Значение указанного заголовка или значение по умолчанию.</returns>
        public static bool GetRequestHeaderValueAsBool(this HttpContext httpContext, string headerKey, 
            bool defaultValue = false)
        {
            if (httpContext.Request.Headers.TryGetValue(headerKey, out var values))
            {
                var value = values.FirstOrDefault() ?? string.Empty;
                return XBooleanConverter.Parse(value);
            }

            return defaultValue;
        }

        /// <summary>
        /// Получение устройство входа пользователя через контекст запроса.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <returns>Устройство входа.</returns>
        public static Device GetDeviceFromRequest(this HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent))
            {
                // get a parser with the embedded regex patterns
                var uaParser = Parser.GetDefault();

                var client_info = uaParser.Parse(userAgent);

                var device = new Device
                {
                    Platform = $"{client_info.OS.Family} {client_info.OS.Major} {client_info.OS.Minor}",
                    Family = client_info.Device.Family,
                    Brand = client_info.Device.Brand,
                    Model = client_info.Device.Model
                };
                device.SetCodeId();

                return device;
            }

            return new Device();
        }

        /// <summary>
        /// Получение названия браузера пользователя через контекст запроса.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        /// <returns>Названия браузера.</returns>
        public static string GetBrowserFromRequest(this HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent))
            {
                // get a parser with the embedded regex patterns
                var uaParser = Parser.GetDefault();

                var client_info = uaParser.Parse(userAgent);

                return client_info.UA.Family;
            }

            return string.Empty;
        }
    }
    /**@}*/
}
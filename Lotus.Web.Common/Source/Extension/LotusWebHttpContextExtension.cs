﻿//=====================================================================================================================
// Проект: Общий модуль платформы Web
// Раздел: Методы расширения
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusWebHttpContextExtension.cs
*		Статический класс реализующий методы расширения для работы с контекстом запроса.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http.Headers;
//=====================================================================================================================
namespace Lotus
{
	namespace Web
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup WebCommonExtension
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для работы с контекстом запроса
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XHttpContextExtension
		{
            //---------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Получение токена доступа через контекст запроса
            /// </summary>
            /// <param name="httpContex">Контекст запроса</param>
            /// <returns>Токен доступа</returns>
            //---------------------------------------------------------------------------------------------------------
            public static String GetAccessToken(this HttpContext httpContex)
            {
                var authorization = httpContex.Request.Headers[HeaderNames.Authorization];

                if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    return headerValue.Parameter ?? String.Empty;
                }

                return String.Empty;
            }
        }
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
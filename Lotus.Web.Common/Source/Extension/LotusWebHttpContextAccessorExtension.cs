﻿//=====================================================================================================================
// Проект: Общий модуль платформы Web
// Раздел: Методы расширения
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusWebHttpContextAccessorExtension.cs
*		Статический класс реализующий методы расширения для работы с провайдером контекста HTTP запроса.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System.Security.Claims;
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
		/// Статический класс реализующий методы расширения для работы с провайдером контекста HTTP запроса
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XHttpContextAccessorExtension
        {
            //---------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Поиск первого значения утверждения с указанным типом
            /// </summary>
            /// <param name="httpContextAccessor">Провайдер контекста HTTP запроса</param>
            /// <returns>Удостоверение, представленное набором утверждений</returns>
            //---------------------------------------------------------------------------------------------------------
            public static ClaimsIdentity? GetClaimsIdentity(this IHttpContextAccessor httpContextAccessor)
			{
                if(httpContextAccessor.HttpContext != null &&
                    httpContextAccessor.HttpContext.User != null)
                {
                    return httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                }

				return null;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
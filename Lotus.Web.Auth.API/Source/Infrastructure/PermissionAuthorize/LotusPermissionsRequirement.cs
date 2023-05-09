﻿//=====================================================================================================================
// Проект: Web API модуля авторизации пользователя
// Раздел: Подсистема инфраструктуры
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusPermissionsRequirement.cs
*		Требование для авторизации на основе разрешений.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using Microsoft.AspNetCore.Authorization;
//=====================================================================================================================
namespace Lotus.Auth
{
	namespace User
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup AuthUserApiInfrastructure
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Требование для авторизации на основе разрешений
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class PermissionsRequirement : IAuthorizationRequirement
		{
			/// <summary>
			/// Набор разрешений
			/// </summary>
			public HashSet<String> Permissions { get; set; } = new HashSet<String>();
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
﻿//=====================================================================================================================
// Проект: Модуль авторизации пользователя
// Раздел: Сущности предметной области
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGroup.cs
*		Класс для определения группы пользователя.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus.Auth
{
	namespace User
	{
		//-------------------------------------------------------------------------------------------------------------
		/** \addtogroup AuthUserEntities
		*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для определения группы пользователя
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CGroup : EntityDb<Int32>
		{
			#region ======================================= МЕТОДЫ ОПРЕДЕЛЕНИЯ МОДЕЛЕЙ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конфигурирование модели для типа <see cref="CGroup"/>
			/// </summary>
			/// <param name="modelBuilder">Интерфейс для построения моделей</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ModelCreating(ModelBuilder modelBuilder)
			{
				// Определение для таблицы
				var model = modelBuilder.Entity<CGroup>();
				model.ToTable("Group", XDbConstants.SchemeName);
			}
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Наименование группы
			/// </summary>
			[MaxLength(40)]
			public String Name { get; set; } = String.Empty;

			/// <summary>
			/// Краткое наименование группы
			/// </summary>
			[MaxLength(20)]
			public String? ShortName { get; set; }

			/// <summary>
			/// Все пользователи
			/// </summary>
			public ICollection<CUser> Users { get; set; } = new HashSet<CUser>();
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/**@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
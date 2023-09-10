﻿//=====================================================================================================================
// Проект: Общий модуль платформы Web
// Раздел: Подсистема сессии
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusWebSession.cs
*		Класс для определения сессии пользователя.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 30.04.2023
//=====================================================================================================================
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
    namespace Web
    {
		//-------------------------------------------------------------------------------------------------------------
		/**
         * \defgroup WebCommonSession Подсистема сессии
         * \ingroup WebCommon
         * \brief Подсистема сессии.
         * @{
         */
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для определения сессии пользователя
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class Session : EntityDb<Guid>
        {
            #region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
            /// <summary>
            /// Имя таблицы
            /// </summary>
            public const String TABLE_NAME = "Session";
			#endregion

			#region ======================================= МЕТОДЫ ОПРЕДЕЛЕНИЯ МОДЕЛЕЙ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конфигурирование модели для типа <see cref="Session"/>
			/// </summary>
			/// <param name="modelBuilder">Интерфейс для построения моделей</param>
			/// <param name="schemeName">Схема куда будет помещена таблица</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ModelCreating(ModelBuilder modelBuilder, String schemeName)
            {
                // Определение для таблицы
                var model = modelBuilder.Entity<Session>();
                model.ToTable(TABLE_NAME, schemeName);
            }
            #endregion

            #region ======================================= СВОЙСТВА ==================================================
            /// <summary>
            /// Наименование браузера
            /// </summary>
            [MaxLength(20)]
            public String? Browser { get; set; }

            /// <summary>
            /// Время начало сессии
            /// </summary>
            public DateTime BeginTime { get; set; }

            /// <summary>
            /// Время окончание сессии
            /// </summary>
            public DateTime? EndTime { get; set; }

            /// <summary>
            /// Идентификатор пользователя 
            /// </summary>
            public Guid? UserId { get; set; }

			/// <summary>
			/// Устройство для входа
			/// </summary>
			[ForeignKey(nameof(DeviceId))]
			public Device? Device { get; set; }

			/// <summary>
			/// Идентификатор устройства для входа
			/// </summary>
			public Int32? DeviceId { get; set; }
            #endregion
        }
        //-------------------------------------------------------------------------------------------------------------
        /**@}*/
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
using System.Diagnostics.CodeAnalysis;
using System.Net;

using Lotus.Core;

using Microsoft.AspNetCore.Mvc;

namespace Lotus.Web
{
    /** \addtogroup WebCommonExtension
    *@{*/
    /// <summary>
    /// Статический класс реализующий методы расширения для работы c IMvcBuilder.
    /// </summary>
    public static class LotusMvcBuilderExtensions
    {
        /// <summary>
        /// Преобразование всех ошибок моделей в тип Result.
        /// </summary>
        /// <param name="mvcBuilder">Построитель контролера.</param>
        /// <returns>Построитель контролера.</returns>
        public static IMvcBuilder AddInvalidModelToResult([NotNull] this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
                options.InvalidModelStateResponseFactory = context =>
                {
                    var resultMessages = new List<ResultMessage>();

                    foreach (var field in context.ModelState)
                    {
                        if (field.Value.Errors.Count > 0)
                        {
                            foreach (var error in field.Value.Errors)
                            {
                                resultMessages.Add(new ResultMessage
                                {
                                    Level = 1,
                                    Text = $"{field.Key}: {error.ErrorMessage}"
                                });
                            }
                        }
                    }

                    var result = Result.Failed(
                        HttpStatusCode.BadRequest,
                        message: "Обнаружены ошибки в данных запроса",
                        code: 400,
                        value: resultMessages
                    );

                    return new BadRequestObjectResult(result);
                };
            });
            return mvcBuilder;

        }
    }
    /**@}*/
}
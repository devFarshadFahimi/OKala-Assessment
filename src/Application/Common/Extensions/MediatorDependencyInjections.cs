using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Application.Common.Extensions;

public static class MediatorDependencyInjections
{
    public static IServiceCollection AddValidationCultureInfo(this IServiceCollection services,
        string culture = "fa-IR", bool suppressModelStateInvalidFilter = false)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(culture);
        ValidatorOptions.Global.DisplayNameResolver = (type, member, expression)
            => member != null
                ? member
                    .GetCustomAttributes(typeof(DisplayAttribute), true)
                    .FirstOrDefault() is not DisplayAttribute displayAttribute
                    ? member.Name //throw new ArgumentNullException($"فیلد '{member.Name}'فاقد نام نمایشی است، لطفا به برنامه نویسان سیستم اطلاع دهید.")
                    : displayAttribute?.Name + ""
                : null;
        return services;
    }

}
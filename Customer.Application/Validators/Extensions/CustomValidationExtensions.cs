using System.Text.RegularExpressions;
using Customer.Application.Services;
using FluentValidation;

namespace Customer.Application.Validators.Extensions;

public static class CustomValidationExtensions
{
    public static IRuleBuilderOptions<T, string> OnlyGeorgianOrLatinLetters
        <T>(this IRuleBuilder<T, string> ruleBuilder, LocalizationService localizer)
    {
        return ruleBuilder
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(50)
            .Must(name =>
                Regex.IsMatch(name, @"^[a-zA-Z]+$") ||
                Regex.IsMatch(name, @"^[ა-ჰ]+$")
            )
            .WithMessage($"{localizer["TextOnlyGeorgianOrEnglish"]}");

    }
    
    public static IRuleBuilderOptions<T, string> ValidPersonalId<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder.Length(11).Matches(@"^\d+$");
    
    public static IRuleBuilderOptions<T, DateTime?> IsAdult<T>(this IRuleBuilder<T, DateTime?> ruleBuilder, LocalizationService localizer)
        => ruleBuilder.Must(date =>
        {
            if (!date.HasValue) return true; 
            var today = DateTime.Today;
            var age = today.Year - date.Value.Year;
            if (date.Value.Date > today.AddYears(-age)) age--;
            return age >= 18;
        }).WithMessage(localizer["InvalidAge"]);

}
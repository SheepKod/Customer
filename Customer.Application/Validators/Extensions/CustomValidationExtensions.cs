using System.Text.RegularExpressions;
using FluentValidation;

namespace Customer.Application.Validators.Extensions;

public static class CustomValidationExtensions
{
    public static IRuleBuilderOptions<T, string> OnlyGeorgianOrLatinLetters
        <T>(this IRuleBuilder<T, string> ruleBuilder, string fieldName)
    {
        return ruleBuilder
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage($"{fieldName} must be between 2 and 50 characters long")
            .Must(name =>
                Regex.IsMatch(name, @"^[a-zA-Z]+$") ||
                Regex.IsMatch(name, @"^[ა-ჰ]+$")
            )
            .WithMessage($"{fieldName} must be either only English (Latin) or only Georgian letters");

    }
    
    public static IRuleBuilderOptions<T, string> ValidPersonalId<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder.Length(11).Matches(@"^\d+$");
    
    public static IRuleBuilderOptions<T, DateTime?> IsAdult<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        => ruleBuilder.Must(date =>
        {
            if (!date.HasValue) return true; 
            var today = DateTime.Today;
            var age = today.Year - date.Value.Year;
            if (date.Value.Date > today.AddYears(-age)) age--;
            return age >= 18;
        }).WithMessage("Customer must be at least 18 years old.");

}
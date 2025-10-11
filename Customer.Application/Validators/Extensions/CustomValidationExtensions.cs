using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Customer.Application.Constants;
using Customer.Application.Resources;
using FluentValidation;

namespace Customer.Application.Validators.Extensions;

public static class CustomValidationExtensions
{
    public static IRuleBuilderOptions<T, string> OnlyGeorgianOrLatinLetters
        <T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(50)
            .Must(name =>
                Regex.IsMatch(name, RegexPatterns.LatinLettersOnly) ||
                Regex.IsMatch(name, RegexPatterns.GeorgianLettersOnly)
            )
            .WithMessage($"{ValidationMessages.TextOnlyGeorgianOrEnglish}");
    }

    public static IRuleBuilderOptions<T, string> ValidPersonalId<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder.Length(11).Matches(RegexPatterns.NumericOnly);

    public static IRuleBuilderOptions<T, DateTime?> IsAdult<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        => ruleBuilder.Must(date =>
            IsAdult(date.Value)).WithMessage(ValidationMessages.InvalidAge);

    public static IRuleBuilderOptions<T, DateTime> IsAdult<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        => ruleBuilder.Must(IsAdult).WithMessage(ValidationMessages.InvalidAge);

    public static bool IsAdult(DateTime date)
    {
        var today = DateTime.Today;
        var age = today.Year - date.Year;
        if (date.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }
}
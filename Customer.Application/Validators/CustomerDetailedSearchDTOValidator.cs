using Customer.Application.Constants;
using Customer.Application.DTOs;
using Customer.Application.Services;
using Customer.Application.Validators.Extensions;
using FluentValidation;

public class CustomerDetailedSearchDTOValidator : AbstractValidator<CustomerDetailedSearchDTO>
{
    public CustomerDetailedSearchDTOValidator(LocalizationService localizer)
    {
        RuleFor(x => x.FirstName)
            .OnlyGeorgianOrLatinLetters(localizer)
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .OnlyGeorgianOrLatinLetters(localizer)
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.PersonalId)
            .ValidPersonalId()
            .When(x => !string.IsNullOrWhiteSpace(x.PersonalId));

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .When(x => x.CustomerId.HasValue);

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender.HasValue);

        RuleFor(x => x.DateOfBirth)
            .Must(date =>
            {
                if (!date.HasValue) return true;
                var today = DateTime.Today;
                var age = today.Year - date.Value.Year;
                if (date.Value.Date > today.AddYears(-age)) age--;
                return age >= 18;
            })
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage(localizer[ValidationMessageKeys.InvalidAge]);

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .When(x => x.CityId.HasValue);

        RuleFor(x => x.PhoneNumber)
            .Matches(RegexPatterns.PhoneNumber)
            .WithMessage(localizer[ValidationMessageKeys.InvalidPhoneNumber])
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.PhoneType)
            .IsInEnum()
            .When(x => x.PhoneType.HasValue);

        RuleFor(x => x.RelatedCustomerId)
            .GreaterThan(0)
            .When(x => x.RelatedCustomerId.HasValue);

        RuleFor(x => x.RelationType)
            .IsInEnum()
            .When(x => x.RelationType.HasValue);
    }
}
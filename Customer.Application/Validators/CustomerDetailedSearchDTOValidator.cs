using Customer.Application.Constants;
using Customer.Application.DTOs;
using Customer.Application.Resources;
using Customer.Application.Validators.Extensions;
using FluentValidation;

public class CustomerDetailedSearchDTOValidator : AbstractValidator<CustomerDetailedSearchDTO>
{
    public CustomerDetailedSearchDTOValidator()
    {
        RuleFor(x => x.FirstName)
            .OnlyGeorgianOrLatinLetters()
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .OnlyGeorgianOrLatinLetters()
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
            .IsAdult()
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage(ValidationMessages.InvalidAge);

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .When(x => x.CityId.HasValue);

        RuleFor(x => x.PhoneNumber)
            .Matches(RegexPatterns.PhoneNumber)
            .WithMessage(ValidationMessages.InvalidPhoneNumber)
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
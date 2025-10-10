using Customer.Application.Dtos;
using Customer.Application.Services;
using Customer.Application.Validators.Extensions;
using FluentValidation;

namespace Customer.Application.Validators;

public class UpdateCustomerDTOValidator : AbstractValidator<UpdateCustomerDTO>
{
    public UpdateCustomerDTOValidator(LocalizationService localizer)
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0);

        RuleFor(x => x.FirstName)
            .OnlyGeorgianOrLatinLetters( localizer)
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .OnlyGeorgianOrLatinLetters(localizer)
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.PersonalId)
            .ValidPersonalId()
            .When(x => !string.IsNullOrWhiteSpace(x.PersonalId));

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender.HasValue);

        RuleFor(x => x.DateOfBirth)
            .IsAdult(localizer)
            .When(x => x.DateOfBirth.HasValue);

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .When(x => x.CityId.HasValue);

        // TODO
        // RuleForEach(x => x.PhoneNumbers)
        //     .SetValidator(new PhoneNumberDTOValidator())
        //     .When(x => x.PhoneNumbers != null);
    }
}
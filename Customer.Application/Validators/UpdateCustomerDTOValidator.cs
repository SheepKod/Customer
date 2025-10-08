using Customer.Application.Dtos;
using Customer.Application.Validators.Extensions;
using FluentValidation;

namespace Customer.Application.Validators;

public class UpdateCustomerDTOValidator : AbstractValidator<UpdateCustomerDTO>
{
    public UpdateCustomerDTOValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId must be a positive number.");

        RuleFor(x => x.FirstName)
            .OnlyGeorgianOrLatinLetters("First Name")
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .OnlyGeorgianOrLatinLetters("Last Name")
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.PersonalId)
            .ValidPersonalId()
            .When(x => !string.IsNullOrWhiteSpace(x.PersonalId));

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender.HasValue);

        RuleFor(x => x.DateOfBirth)
            .IsAdult()
            .When(x => x.DateOfBirth.HasValue);

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .When(x => x.CityId.HasValue)
            .WithMessage("CityId must be a positive number.");

        // TODO
        // RuleForEach(x => x.PhoneNumbers)
        //     .SetValidator(new PhoneNumberDTOValidator())
        //     .When(x => x.PhoneNumbers != null);
    }
}
using System.ComponentModel;
using Customer.Application.Dtos;
using Customer.Application.Validators.Extensions;
using FluentValidation;

namespace Customer.Application.Validators;

public class AddCustomerDTOValidator : AbstractValidator<AddCustomerDTO>
{
    public AddCustomerDTOValidator()
    {
        RuleFor(c => c.FirstName).OnlyGeorgianOrLatinLetters();
        RuleFor(c => c.LastName).OnlyGeorgianOrLatinLetters();
        RuleFor(c => c.Gender).NotNull().IsInEnum();
        RuleFor(c => c.PersonalId).NotNull().NotEmpty().ValidPersonalId();
        RuleFor(c => c.DateOfBirth).NotNull().NotEmpty().IsAdult();
        RuleFor(c => c.CityId).NotNull().NotEmpty().GreaterThan(0);
        RuleForEach(c => c.PhoneNumbers).SetValidator(new PhoneNumberDTOValidator());
    }
}
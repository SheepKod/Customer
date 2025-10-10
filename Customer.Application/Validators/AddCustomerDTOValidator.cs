using System.ComponentModel;
using System.Text.RegularExpressions;
using Customer.Application.Constants;
using Customer.Application.Dtos;
using Customer.Application.Services;
using Customer.Application.Validators.Extensions;
using FluentValidation;

namespace Customer.Application.Validators;

public class AddCustomerDTOValidator:AbstractValidator<AddCustomerDTO>
{
    public AddCustomerDTOValidator(LocalizationService localizer)
    {
        RuleFor(c=> c.FirstName).OnlyGeorgianOrLatinLetters(localizer);
        RuleFor(c=> c.LastName).OnlyGeorgianOrLatinLetters(localizer);
        RuleFor(c=> c.Gender).NotNull().IsInEnum();
        RuleFor(c=> c.PersonalId).NotNull().NotEmpty().ValidPersonalId();
        RuleFor(c => c.DateOfBirth).NotNull().NotEmpty().IsAdult(localizer);
        RuleFor(c => c.CityId).NotNull().NotEmpty().GreaterThan(0);
        RuleForEach(c => c.PhoneNumbers).SetValidator(new PhoneNumberDTOValidator(localizer));

    }
}
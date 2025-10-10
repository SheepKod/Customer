using System.ComponentModel;
using System.Text.RegularExpressions;
using Customer.Application.Dtos;
using Customer.Application.Validators.Extensions;
using FluentValidation;

namespace Customer.Application.Validators;

public class AddCustomerDTOValidator:AbstractValidator<AddCustomerDTO>
{
    public AddCustomerDTOValidator()
    {
        RuleFor(c=> c.FirstName).OnlyGeorgianOrLatinLetters("First Name");
        RuleFor(c=> c.LastName).OnlyGeorgianOrLatinLetters("Last Name");
        RuleFor(c=> c.Gender).NotNull().NotEmpty().IsInEnum();
        RuleFor(c=> c.PersonalId).NotNull().NotEmpty().ValidPersonalId();
        RuleFor(c => c.DateOfBirth).NotNull().NotEmpty().Must(date =>
        {
         
            var today = DateTime.Today;
            var age = today.Year - date.Year;
            if (date.Date > today.AddYears(-age)) age--;
            return age >= 18;
        }).WithMessage("Customer must be at least 18 years old.");
        RuleFor(c=> c.CityId).NotNull().NotEmpty().GreaterThan(0).WithMessage("CityId must be a positive number");;;
        RuleForEach(c => c.PhoneNumbers).SetValidator(new PhoneNumberDTOValidator());
        // RuleFor(c=> c.ImagePath).
        //     Must(value=> value== null || Guid.TryParse(value, out _))
        //     .WithMessage("ImagePath must be either null or a valid GUID.");




    }
}
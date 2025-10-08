using Customer.Application.Dtos;
using FluentValidation;

namespace Customer.Application.Validators;

public class PhoneNumberDTOValidator : AbstractValidator<PhoneNumberDTO>
{
    public PhoneNumberDTOValidator()
    {
        RuleFor(p => p.Number)
            .NotEmpty()
            .Matches(@"^\d{4,50}$") 
            .WithMessage("Phone Number must be between 4 and 50 digits long and contain only digits");
        RuleFor(p => p.Type)
            .IsInEnum();
    }
}
using Customer.Application.Dtos;
using Customer.Application.Services;
using FluentValidation;

namespace Customer.Application.Validators;

public class PhoneNumberDTOValidator : AbstractValidator<PhoneNumberDTO>
{
    public PhoneNumberDTOValidator(LocalizationService localizer)
    {
        RuleFor(p => p.Number)
            .NotEmpty()
            .Matches(@"^\d{4,50}$")
            .WithMessage(localizer["InvalidPhoneNumber"]);
        RuleFor(p => p.Type)
            .IsInEnum();
    }
}
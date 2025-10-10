using Customer.Application.Constants;
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
            .Matches(RegexPatterns.PhoneNumber)
            .WithMessage(localizer[ValidationMessageKeys.InvalidPhoneNumber]);
        RuleFor(p => p.Type)
            .IsInEnum();
    }
}
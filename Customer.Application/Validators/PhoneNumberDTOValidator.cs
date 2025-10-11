using Customer.Application.Constants;
using Customer.Application.Dtos;
using Customer.Application.Resources;
using FluentValidation;

namespace Customer.Application.Validators;

public class PhoneNumberDTOValidator : AbstractValidator<PhoneNumberDTO>
{
    public PhoneNumberDTOValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .When(p => p.Id.HasValue);
        RuleFor(p => p.Number)
            .NotEmpty()
            .Matches(RegexPatterns.PhoneNumber)
            .WithMessage(ValidationMessages.InvalidPhoneNumber);
        RuleFor(p => p.Type)
            .IsInEnum();
    }
}
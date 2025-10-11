using Customer.Application.Constants;
using Customer.Application.DTOs;
using Customer.Application.Resources;
using FluentValidation;

namespace Customer.Application.Validators;

public class UpdatePhoneNumberDTOValidator : AbstractValidator<UpdatePhoneNumberDTO>
{
    public UpdatePhoneNumberDTOValidator()
    {
        RuleFor(p => p.Id)
            .GreaterThan(0);

        RuleFor(p => p.Number)
            .Matches(RegexPatterns.PhoneNumber)
            .WithMessage(ValidationMessages.InvalidPhoneNumber)
            .When(p => !string.IsNullOrWhiteSpace(p.Number));

        RuleFor(p => p.Type)
            .IsInEnum()
            .When(p => p.Type.HasValue);
    }
}

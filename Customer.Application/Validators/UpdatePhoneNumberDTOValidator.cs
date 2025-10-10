using Customer.Application.Constants;
using Customer.Application.DTOs;
using Customer.Application.Services;
using FluentValidation;

namespace Customer.Application.Validators;

public class UpdatePhoneNumberDTOValidator : AbstractValidator<UpdatePhoneNumberDTO>
{
    public UpdatePhoneNumberDTOValidator(LocalizationService localizer)
    {
        RuleFor(p => p.Id)
            .GreaterThan(0);

        RuleFor(p => p.Number)
            .Matches(RegexPatterns.PhoneNumber)
            .WithMessage(localizer[ValidationMessageKeys.InvalidPhoneNumber])
            .When(p => !string.IsNullOrWhiteSpace(p.Number));

        RuleFor(p => p.Type)
            .IsInEnum()
            .When(p => p.Type.HasValue);
    }
}

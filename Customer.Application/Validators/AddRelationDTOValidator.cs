using Customer.Application.Constants;
using Customer.Application.Dtos;
using Customer.Application.Services;
using FluentValidation;

namespace Customer.Application.Validators;

public class AddRelationDTOValidator : AbstractValidator<AddRelationDTO>
{
    public AddRelationDTOValidator(LocalizationService localizer)
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0);

        RuleFor(x => x.RelatedCustomerId)
            .GreaterThan(0)
            .NotEqual(x => x.CustomerId)
            .WithMessage(localizer[ValidationMessageKeys.RelatedCustomerIdCannotBeTheSameAsTheCurrentCustomer]);

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}

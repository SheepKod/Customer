using Customer.Application.Dtos;
using Customer.Application.Resources;
using FluentValidation;

namespace Customer.Application.Validators;

public class AddRelationDTOValidator : AbstractValidator<AddRelationDTO>
{
    public AddRelationDTOValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0);

        RuleFor(x => x.RelatedCustomerId)
            .GreaterThan(0)
            .NotEqual(x => x.CustomerId)
            .WithMessage(ValidationMessages.RelatedCustomerIdCannotBeTheSameAsTheCurrentCustomer);

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}

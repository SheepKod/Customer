using Customer.Application.Dtos;
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
            .WithMessage("Customer cannot have a relation to themselves.");

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}

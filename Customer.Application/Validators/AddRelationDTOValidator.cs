using Customer.Application.Dtos;
using FluentValidation;

namespace Customer.Application.Validators;

public class AddRelationDTOValidator : AbstractValidator<AddRelationDTO>
{
    public AddRelationDTOValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId must be a positive number.");

        RuleFor(x => x.RelatedCustomerId)
            .GreaterThan(0)
            .WithMessage("RelatedCustomerId must be a positive number.")
            .NotEqual(x => x.CustomerId)
            .WithMessage("Customer cannot have a relation to themselves.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("RelationType is invalid.");
    }
}

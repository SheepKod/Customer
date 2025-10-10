using Customer.Application.Dtos;
using Customer.Application.DTOs;
using FluentValidation;

namespace Customer.Application.Validators;

public class PagingDTOValidator : AbstractValidator<PagingDTO>
{
    public PagingDTOValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}
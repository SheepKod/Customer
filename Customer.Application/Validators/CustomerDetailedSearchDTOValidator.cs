using Customer.Application.DTOs;
using Customer.Application.Validators.Extensions;
using FluentValidation;

public class CustomerDetailedSearchDTOValidator : AbstractValidator<CustomerDetailedSearchDTO>
{
    public CustomerDetailedSearchDTOValidator()
    {
        RuleFor(x => x.FirstName)
            .OnlyGeorgianOrLatinLetters("First Name")
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));
        
        RuleFor(x => x.LastName)
            .OnlyGeorgianOrLatinLetters("Last Name")
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));
        
        RuleFor(x => x.PersonalId)
            .ValidPersonalId()
            .When(x => !string.IsNullOrWhiteSpace(x.PersonalId));

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .When(x => x.CustomerId.HasValue)
            .WithMessage("CustomerId must be a positive number.");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender.HasValue);
        
        RuleFor(x => x.DateOfBirth)
            .Must(date =>
            {
                if (!date.HasValue) return true; 
                var today = DateTime.Today;
                var age = today.Year - date.Value.Year;
                if (date.Value.Date > today.AddYears(-age)) age--;
                return age >= 18;
            })
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Customer must be at least 18 years old.");
        
        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .When(x => x.CityId.HasValue)
            .WithMessage("CityId must be a positive number.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\d{4,50}$")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number must be 4-50 digits.");

        RuleFor(x => x.PhoneType)
            .IsInEnum()
            .When(x => x.PhoneType.HasValue);

        RuleFor(x => x.RelatedCustomerId)
            .GreaterThan(0)
            .When(x => x.RelatedCustomerId.HasValue)
            .WithMessage("RelatedCustomerId must be a positive number.");
        
        RuleFor(x => x.RelationType)
            .IsInEnum()
            .When(x => x.RelationType.HasValue);
    }
}
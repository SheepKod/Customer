using Customer.Domain.Enums;

namespace Customer.Domain.Models;

public class IndividualCustomer
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public Gender Gender { get; init; }
    public string PersonalId { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public int CityId { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; init; } = [];
    public List<Relation> Relations { get; init; } = [];
    public string ImagePath { get; init; } = string.Empty;
}
using Customer.Domain.Enums;

namespace Customer.Domain.Models;

public class IndividualCustomer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string PersonalId { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int CityId { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; set; } = [];
    public List<Relation> Relations { get; set; } = [];
    public Guid ImageKey { get; set; }
}
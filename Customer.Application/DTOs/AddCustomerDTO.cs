using Customer.Domain.Enums;
using Customer.Domain.Models;

namespace Customer.Application.Dtos;

public class AddCustomerDTO
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public Gender Gender { get; init; }
    public string PersonalId { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public int CityId { get; init; }
    public List<PhoneNumber> PhoneNumbers { get; set; } = [];
    public string ImagePath { get; init; } = string.Empty;
}
using Customer.Domain.Enums;
using Customer.Domain.Models;

namespace Customer.Application.Dtos;

public class AddCustomerDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string PersonalId { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int CityId { get; set; }
    public List<PhoneNumberDTO> PhoneNumbers { get; set; } = [];
}
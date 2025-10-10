using Customer.Application.DTOs;
using Customer.Domain.Enums;
using Customer.Domain.Models;

namespace Customer.Application.Dtos;

public class UpdateCustomerDTO
{
    public int CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; } 
    public Gender? Gender { get; set; }
    public string? PersonalId { get; set; } 
    public DateTime? DateOfBirth { get; set; }
    public int? CityId { get; set; }
    public List<UpdatePhoneNumberDTO>? PhoneNumbers { get; set; }
}
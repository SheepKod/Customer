using Customer.Domain.Enums;
using Customer.Domain.Models;

namespace Customer.Application.DTOs;

public class CustomerDetailedSearchDTO
{
    public int? CustomerId { get; set; }
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public string? PersonalId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? CityId { get; set; }
    public string? PhoneNumber { get; set; } 
    public PhoneType? PhoneType { get; set; }
    public int? RelatedCustomerId { get; set; }
    public RelationType? RelationType { get; set; }
}
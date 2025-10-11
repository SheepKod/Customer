using Customer.Application.Dtos;
using Customer.Domain.Enums;
using Customer.Domain.Models;

namespace Customer.Application.DTOs;

public class IndividualCustomerSearchResultDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string PersonalId { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int CityId { get; set; }
    public List<PhoneNumberDTO> PhoneNumbers { get; set; } = [];
    public List<RelationDTO> Relations { get; set; } = [];
    public string? ImageUrl { get; set; }
}
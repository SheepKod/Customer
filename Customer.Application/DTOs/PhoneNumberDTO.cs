using Customer.Domain.Enums;

namespace Customer.Application.Dtos;

public class PhoneNumberDTO
{
    public int? Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public PhoneType Type { get; set; }
}
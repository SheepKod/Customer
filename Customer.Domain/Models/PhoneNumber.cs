using Customer.Domain.Enums;

namespace Customer.Domain.Models;

public class PhoneNumber
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public PhoneType Type { get; set; }
    public int IndividualCustomerId { get; set; }
}
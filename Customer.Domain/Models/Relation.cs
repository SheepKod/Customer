using Customer.Domain.Enums;

namespace Customer.Domain.Models;

public class Relation
{
    public int Id { get; set; }
    public int IndividualCustomerId { get; set; }
    public int RelatedCustomerId { get; set; }
    public RelationType Type { get; set; }
}
using Customer.Domain.Enums;

namespace Customer.Application.Dtos;

public class RelationDto
{
    public int CustomerId { get; set; }
    public int RelatedCustomerId { get; set; }
    public RelationType Type { get; set; }
}
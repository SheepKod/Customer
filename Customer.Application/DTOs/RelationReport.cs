using Customer.Domain.Enums;

namespace Customer.Application.DTOs;

public class RelationReport
{
    public RelationType Type { get; set; }
    public int Count { get; set; } 
}
using Customer.Application.Dtos;
using Customer.Domain.Enums;
using Customer.Domain.Models;


namespace Customer.Application.Abstractions;


public interface ICustomerRepository
{
   public Task<int> AddCustomer(IndividualCustomer customer);
   public Task<IndividualCustomer?> GetCustomerById(int customerId);
   public Task DeleteCustomer(IndividualCustomer customerId);
   public Task<IndividualCustomer?> GetCustomerFullDetailsById(int customerId);
   public Task<IndividualCustomer?> GetCustomerByPersonalId(string personalId);
   
   public Task<int> AddRelation(Relation relation);
   public Task<bool> RelationExists(int customerId, int relatedCustomerId, RelationType type);
   public Task DeleteRelation(Relation relation);
   public Task<Relation?> GetRelationById(int relationId);
   
   public Task SaveChangesAsync();

}
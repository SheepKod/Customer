using Customer.Application.Dtos;
using Customer.Application.DTOs;
using Customer.Domain.Enums;
using Customer.Domain.Models;


namespace Customer.Application.Abstractions;


public interface ICustomerRepository
{
   public Task<int> AddCustomer(IndividualCustomer customer, CancellationToken cancellationToken);
   public Task<City> GetCityById(int id, CancellationToken cancellationToken);
   public Task<IndividualCustomer?> GetCustomerById(int customerId,CancellationToken cancellationToken);
   public Task DeleteCustomer(IndividualCustomer customerId,CancellationToken cancellationToken);
   public Task<IndividualCustomer?> GetCustomerFullDetailsById(int customerId,CancellationToken cancellationToken);
   public Task<int> AddRelation(Relation relation,CancellationToken cancellationToken);
   public Task<bool> RelationExists(int customerId, int relatedCustomerId, RelationType type,CancellationToken cancellationToken);
   public Task DeleteRelation(Relation relation,CancellationToken cancellationToken);
   public Task<Relation?> GetRelationById(int relationId,CancellationToken cancellationToken);
   public Task<PagedResult<IndividualCustomer>> SearchCustomers(CustomerDetailedSearchDTO search, PagingDTO paging,CancellationToken cancellationToken);
   
   public Task SaveChangesAsync(CancellationToken cancellationToken);

}
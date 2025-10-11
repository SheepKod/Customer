using Customer.Application.Dtos;
using Customer.Application.DTOs;
using Customer.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Customer.Application.Abstractions;

public interface ICustomerService
{

    public Task<int> AddCustomer(AddCustomerDTO customer, CancellationToken cancellationToken);
    public Task<int> AddRelation(RelationDTO relation,CancellationToken cancellationToken);
    public Task UpdateCustomer(UpdateCustomerDTO updatedCustomerData,CancellationToken cancellationToken);
    public Task DeleteCustomer(int customerId,CancellationToken cancellationToken);
    public Task DeleteRelation(int relationId,CancellationToken cancellationToken);

    public Task<PagedResult<IndividualCustomerSearchResultDTO>> SearchCustomers(CustomerDetailedSearchDTO search, PagingDTO paging,CancellationToken cancellationToken);
    public Task UploadImage(int customerId, IFormFile image,CancellationToken cancellationToken);
    public Task<List<RelationReport>> GetRelationReport(int customerId,CancellationToken cancellationToken);

}
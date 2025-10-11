using Customer.Application.Dtos;
using Customer.Application.DTOs;
using Customer.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Customer.Application.Abstractions;

public interface ICustomerService
{

    public Task<int> AddCustomer(AddCustomerDTO customer);
    public Task<int> AddRelation(RelationDTO relation);
    public Task UpdateCustomer(UpdateCustomerDTO updatedCustomerData);
    public Task DeleteCustomer(int customerId);
    public Task DeleteRelation(int relationId);

    public Task<PagedResult<IndividualCustomerSearchResultDTO>> SearchCustomers(CustomerDetailedSearchDTO search, PagingDTO paging);
    public Task UploadImage(int customerId, IFormFile image);
    public Task<List<RelationReport>> GetRelationReport(int customerId);

}
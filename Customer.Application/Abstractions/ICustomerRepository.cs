using Customer.Application.Dtos;
using Customer.Domain.Models;


namespace Customer.Application.Abstractions;


public interface ICustomerRepository
{
   public Task<RetailCustomer> AddCustomer(AddCustomerDTO customer);
}
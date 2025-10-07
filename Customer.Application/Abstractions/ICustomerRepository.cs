using Customer.Application.Dtos;
using Customer.Domain.Models;


namespace Customer.Application.Abstractions;


public interface ICustomerRepository
{
   public Task<int> AddCustomer(IndividualCustomer customer);
   public Task<IndividualCustomer?> GetCustomerById(int customerId);
   public Task DeleteCustomer(IndividualCustomer customerId);
   public Task<IndividualCustomer?> GetCustomerFullDetailsById(int customerId);
   public Task<IndividualCustomer?> GetCustomerByPersonalId(string personalId);
}
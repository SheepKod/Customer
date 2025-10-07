using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Domain.Models;

namespace Customer.Infrastructure;

public class CustomerRepository(ApplicationDbContext context): ICustomerRepository
{
    
    // TODO: Continue implementation
    public async Task<IndividualCustomer?> AddCustomer(IndividualCustomer customer)
    {
        
       await context.RetailCustomers.AddAsync(customer);

       foreach (var phoneNumber in customer.PhoneNumbers)
       {
           await context.PhoneNumbers.AddAsync(phoneNumber);
       }
       
       
       var result = await context.SaveChangesAsync();
       if (result <= 0)
       {
           return null;
       }
       return customer;
    }
}
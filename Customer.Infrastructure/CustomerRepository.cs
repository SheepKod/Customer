using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Domain.Models;

namespace Customer.Infrastructure;

public class CustomerRepository(ApplicationDbContext context): ICustomerRepository
{
    
    // TODO: Continue implementation
    public async Task<IndividualCustomer?> AddCustomer(IndividualCustomer customer)
    {
        
       var newCustomer = await context.RetailCustomers.AddAsync(customer);

       foreach (var phoneNumber in customer.PhoneNumbers)
       {
           var newPhoneNumber = new PhoneNumber
           {
               CustomerId = newCustomer.Entity.Id,
               Number = phoneNumber.Number,
               Type = phoneNumber.Type
           };
           await context.PhoneNumbers.AddAsync(newPhoneNumber);
       }
       
       
       var result = await context.SaveChangesAsync();
       if (result <= 0)
       {
           return null;
       }
       return customer;
    }
}
using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Domain.Models;

namespace Customer.Infrastructure;

public class CustomerRepository(ApplicationDbContext context): ICustomerRepository
{
    
    // TODO: Continue implementation
    public async Task<IndividualCustomer> AddCustomer(AddCustomerDTO customer)
    {
        var newCustomer = new IndividualCustomer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Gender = customer.Gender,
            PersonalId = customer.PersonalId,
            DateOfBirth = customer.DateOfBirth,
            CityId = customer.CityId
        };
       await context.RetailCustomers.AddAsync(newCustomer);

       foreach (var phoneNumber in customer.PhoneNumbers)
       {
           await context.PhoneNumbers.AddAsync(phoneNumber);
       }
       
       var result = await context.SaveChangesAsync();
       return newCustomer;
    }
}
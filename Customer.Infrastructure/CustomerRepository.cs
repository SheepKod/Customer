using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure;

public class CustomerRepository(ApplicationDbContext context): ICustomerRepository
{
    
    // TODO: Continue implementation
    public async Task<int> AddCustomer(IndividualCustomer customer)
    {
        
       var newCustomer = await context.RetailCustomers.AddAsync(customer);
       
       await context.SaveChangesAsync();
       return newCustomer.Entity.Id;
    }

    public async Task DeleteCustomer(int customerId)
    {
        var customer = await context.RetailCustomers.FirstAsync(c => c.Id == customerId);
        context.RetailCustomers.Remove(customer);
        await context.SaveChangesAsync();
    }
}
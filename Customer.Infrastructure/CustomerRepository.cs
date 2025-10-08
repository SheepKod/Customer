using Customer.Application.Abstractions;
using Customer.Domain.Enums;
using Customer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure;

public class CustomerRepository(ApplicationDbContext context): ICustomerRepository
{
    public async Task<int> AddCustomer(IndividualCustomer customer)
    {
        
       var newCustomer = await context.RetailCustomers.AddAsync(customer);
       
       await context.SaveChangesAsync();
       return newCustomer.Entity.Id;
    }

    public async Task<IndividualCustomer?> GetCustomerById(int customerId)
    {
        var customer = await context.RetailCustomers.FirstOrDefaultAsync(c => c.Id == customerId);
        return customer;
    }
    
    public async Task<IndividualCustomer?> GetCustomerByPersonalId(string personalId)
    {
        var customer = await context.RetailCustomers.FirstOrDefaultAsync(c => c.PersonalId == personalId);
        return customer;
    }

    public async Task<int> AddRelation(Relation relation)
    {
        var newRelation = await context.AddAsync(relation);
        await context.SaveChangesAsync();
        return relation.Id;

    }

    public async Task DeleteRelation(Relation relation)
    {
        context.Relations.Remove(relation);
        await context.SaveChangesAsync();
    }

    public async Task<Relation?> GetRelationById(int relationId)
    {
        return await context.Relations.FirstOrDefaultAsync(r => r.Id == relationId);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<bool> RelationExists(int customerId, int relatedCustomerId, RelationType type)
    {
        return await context.Relations.AnyAsync(r =>
            r.CustomerId == customerId &&
            r.RelatedCustomerId == relatedCustomerId &&
            r.Type == type);
    }


    public async Task<IndividualCustomer?> GetCustomerFullDetailsById(int customerId)
    {
        var customer = await context.RetailCustomers
            .Include(c=> c.PhoneNumbers)
            .Include(c=> c.Relations)
            .FirstOrDefaultAsync(c => c.Id == customerId);
        return customer;
    }

    public async Task DeleteCustomer(IndividualCustomer customer)
    {
        context.RetailCustomers.Remove(customer);
        await context.SaveChangesAsync();
    }
}
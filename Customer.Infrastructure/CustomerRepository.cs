using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Application.DTOs;
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

    public async Task<City?> GetCityById(int cityId)
    {
        return await context.Cities.FirstOrDefaultAsync(c => c.Id == cityId);
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

    public async Task<PagedResult<IndividualCustomer>> SearchCustomers(CustomerDetailedSearchDTO search, PagingDTO paging)
    {
        var query = context.RetailCustomers.AsQueryable().AsNoTracking();
        
        if(!string.IsNullOrWhiteSpace(search.FirstName))
            query = query.Where(c => EF.Functions.Like(c.FirstName, $"%{search.FirstName}%"));;
        if(!string.IsNullOrWhiteSpace(search.LastName))
            query = query.Where(c => EF.Functions.Like(c.LastName, $"%{search.LastName}%"));
        if(!string.IsNullOrWhiteSpace(search.PersonalId))
            query = query.Where(c => EF.Functions.Like(c.PersonalId, $"%{search.PersonalId}%"));
        if(search.CustomerId.HasValue)
            query = query.Where(c => c.Id == search.CustomerId);
        if(search.Gender.HasValue)
            query = query.Where(c => c.Gender == search.Gender);
        if(search.DateOfBirth.HasValue)
            query = query.Where(c => c.DateOfBirth == search.DateOfBirth);
        if(search.CityId.HasValue)
            query = query.Where(c => c.CityId == search.CityId);
        if(search.RelatedCustomerId.HasValue)
            query = query.Where(c => c.Relations.Any(r => r.RelatedCustomerId == search.RelatedCustomerId));
        if(search.RelationType.HasValue)
            query = query.Where(c => c.Relations.Any(r => r.Type == search.RelationType));
        if (!string.IsNullOrWhiteSpace(search.PhoneNumber))
            query = query.Where(c => c.PhoneNumbers.Any(p => p.Number == search.PhoneNumber));
        if (search.PhoneType.HasValue)
            query = query.Where(c => c.PhoneNumbers.Any(p => p.Type == search.PhoneType));
        
                
        
        var totalCount = await query.CountAsync();

        var customersFound = await query
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .ToListAsync();

        return new PagedResult<IndividualCustomer>
        {
            TotalCount = totalCount,
            Results = customersFound
        };

    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<bool> RelationExists(int customerId, int relatedCustomerId, RelationType type)
    {
        return await context.Relations.AnyAsync(r =>
            r.IndividualCustomerId == customerId &&
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
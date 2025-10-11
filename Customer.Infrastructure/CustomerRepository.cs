using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Application.DTOs;
using Customer.Domain.Enums;
using Customer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    public async Task<int> AddCustomer(IndividualCustomer customer, CancellationToken cancellationToken)
    {
        var newCustomer = await context.RetailCustomers.AddAsync(customer, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        return newCustomer.Entity.Id;
    }

    public async Task<City?> GetCityById(int cityId, CancellationToken cancellationToken)
    {
        return await context.Cities.FirstOrDefaultAsync(c => c.Id == cityId, cancellationToken);
    }

    public async Task<IndividualCustomer?> GetCustomerById(int customerId, CancellationToken cancellationToken)
    {
        var customer = await context.RetailCustomers.FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
        return customer;
    }

    public async Task<int> AddRelation(Relation relation, CancellationToken cancellationToken)
    {
        await context.AddAsync(relation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return relation.Id;
    }

    public async Task DeleteRelation(Relation relation, CancellationToken cancellationToken)
    {
        context.Relations.Remove(relation);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Relation?> GetRelationById(int relationId, CancellationToken cancellationToken)
    {
        return await context.Relations.FirstOrDefaultAsync(r => r.Id == relationId, cancellationToken);
    }

    public async Task<PagedResult<IndividualCustomer>> SearchCustomers(CustomerDetailedSearchDTO search,
        PagingDTO paging, CancellationToken cancellationToken)
    {
        var query = context.RetailCustomers.AsQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search.FirstName))
            query = query.Where(c => EF.Functions.Like(c.FirstName, $"%{search.FirstName}%"));
        
        if (!string.IsNullOrWhiteSpace(search.LastName))
            query = query.Where(c => EF.Functions.Like(c.LastName, $"%{search.LastName}%"));
        if (!string.IsNullOrWhiteSpace(search.PersonalId))
            query = query.Where(c => EF.Functions.Like(c.PersonalId, $"%{search.PersonalId}%"));
        if (search.CustomerId.HasValue)
            query = query.Where(c => c.Id == search.CustomerId);
        if (search.Gender.HasValue)
            query = query.Where(c => c.Gender == search.Gender);
        if (search.DateOfBirth.HasValue)
            query = query.Where(c => c.DateOfBirth == search.DateOfBirth);
        if (search.CityId.HasValue)
            query = query.Where(c => c.CityId == search.CityId);
        if (search.RelatedCustomerId.HasValue)
            query = query.Where(c => c.Relations.Any(r => r.RelatedCustomerId == search.RelatedCustomerId));
        if (search.RelationType.HasValue)
            query = query.Where(c => c.Relations.Any(r => r.Type == search.RelationType));
        if (!string.IsNullOrWhiteSpace(search.PhoneNumber))
            query = query.Where(c => c.PhoneNumbers.Any(p => p.Number == search.PhoneNumber));
        if (search.PhoneType.HasValue)
            query = query.Where(c => c.PhoneNumbers.Any(p => p.Type == search.PhoneType));


        var totalCount = await query.CountAsync(cancellationToken);

        var customersFound = await query
            .Include(c => c.PhoneNumbers)
            .Include(c => c.Relations)
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<IndividualCustomer>
        {
            TotalCount = totalCount,
            Results = customersFound,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize,
        };
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> RelationExists(int customerId, int relatedCustomerId, RelationType type, CancellationToken cancellationToken)
    {
        return await context.Relations.AnyAsync(r =>
            r.IndividualCustomerId == customerId &&
            r.RelatedCustomerId == relatedCustomerId &&
            r.Type == type, cancellationToken);
    }


    public async Task<IndividualCustomer?> GetCustomerFullDetailsById(int customerId, CancellationToken cancellationToken)
    {
        var customer = await context.RetailCustomers
            .Include(c => c.PhoneNumbers)
            .Include(c => c.Relations)
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
        return customer;
    }

    public async Task DeleteCustomer(IndividualCustomer customer, CancellationToken cancellationToken)
    {
        context.RetailCustomers.Remove(customer);
        await context.SaveChangesAsync(cancellationToken);
    }
}
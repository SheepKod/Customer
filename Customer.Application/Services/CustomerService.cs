using Amazon.S3;
using Amazon.S3.Model;
using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Application.DTOs;
using Customer.Application.Exceptions;
using Customer.Domain.Enums;
using Customer.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Customer.Application.Services;

public class CustomerService(ICustomerRepository repo, IAmazonS3 amazonS3) : ICustomerService
{
    public async Task<int> AddCustomer(AddCustomerDTO customer, CancellationToken cancellationToken)
    {
        var convertedPhoneNumbers = ConvertPhoneNumbers(customer.PhoneNumbers);
        var city = await repo.GetCityById(customer.CityId, cancellationToken);
        if (city == null)
            throw new NotFoundException($"City with ID: {customer.CityId} not found");
        var newCustomer = new IndividualCustomer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Gender = customer.Gender,
            PersonalId = customer.PersonalId,
            DateOfBirth = customer.DateOfBirth,
            CityId = customer.CityId,
            PhoneNumbers = convertedPhoneNumbers,
        };
        var customerId = await repo.AddCustomer(newCustomer, cancellationToken);

        return customerId;
    }

    public async Task UpdateCustomer(UpdateCustomerDTO updatedCustomerData, CancellationToken cancellationToken)
    {
        var customer = await repo.GetCustomerFullDetailsById(updatedCustomerData.CustomerId, cancellationToken);
        if (customer == null)
            throw new NotFoundException($"Customer with ID: {updatedCustomerData.CustomerId} not found");

        if (updatedCustomerData.FirstName != null) customer.FirstName = updatedCustomerData.FirstName;

        if (updatedCustomerData.PersonalId != null) customer.PersonalId = updatedCustomerData.PersonalId;

        if (updatedCustomerData.LastName != null) customer.LastName = updatedCustomerData.LastName;

        if (updatedCustomerData.Gender.HasValue) customer.Gender = updatedCustomerData.Gender.Value;

        if (updatedCustomerData.DateOfBirth.HasValue) customer.DateOfBirth = updatedCustomerData.DateOfBirth.Value;

        if (updatedCustomerData.CityId.HasValue)
        {
            var city = await repo.GetCityById(customer.CityId,cancellationToken);
            if (city == null) throw new NotFoundException($"City with ID: {customer.CityId} not found");
            customer.CityId = updatedCustomerData.CityId.Value;
        }


        if (updatedCustomerData.PhoneNumbers != null && updatedCustomerData.PhoneNumbers.Any())
        {
            UpdatePhoneNumbers(customer.PhoneNumbers, updatedCustomerData.PhoneNumbers);
        }

        await repo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCustomer(int customerId, CancellationToken cancellationToken)
    {
        var customer = await repo.GetCustomerById(customerId, cancellationToken);
        if (customer == null) throw new NotFoundException($"Customer with ID: {customerId} not found");
        await repo.DeleteCustomer(customer,cancellationToken);
    }

    public async Task UploadImage(int customerId, IFormFile image, CancellationToken cancellationToken)
    {
        // TODO: bucket name unda gavitano samde readonly constanshi
        var customer = await repo.GetCustomerById(customerId, cancellationToken);
        if (customer == null) throw new NotFoundException($"Customer with ID: {customerId} not found");
        if (customer.ImageKey != null)
        {
            try
            {
                await amazonS3.DeleteObjectAsync("tbc-customer-img", $"{customer.ImageKey}", cancellationToken);
            }
            catch (AmazonS3Exception ex) when (ex.ErrorCode == StatusCodes.Status404NotFound.ToString())
            {
                throw new NotFoundException($"Image with ID : {customer.ImageKey} not found in S3", ex);
            }

            customer.ImageKey = null;
            await repo.SaveChangesAsync(cancellationToken);
        }

        var fileKey = Guid.NewGuid();
        await using (var stream = image.OpenReadStream())
        {
            var request = new PutObjectRequest
            {
                BucketName = "tbc-customer-img",
                Key = fileKey.ToString(),
                InputStream = stream,
                ContentType = image.ContentType
            };
            await amazonS3.PutObjectAsync(request, cancellationToken);
        }

        customer.ImageKey = fileKey;
        await repo.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<IndividualCustomerSearchResultDTO>> SearchCustomers(CustomerDetailedSearchDTO search,
        PagingDTO paging, CancellationToken cancellationToken)
    {
        // TODO: Optimize for large data
        var customers = await repo.SearchCustomers(search, paging, cancellationToken);

        var searchResult = new PagedResult<IndividualCustomerSearchResultDTO>
        {
            TotalCount = customers.TotalCount,
            PageNumber = customers.PageNumber,
            PageSize = customers.PageSize,
            Results = []
        };
        foreach (var customer in customers.Results)
        {
            var customerDto = new IndividualCustomerSearchResultDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PersonalId = customer.PersonalId,
                Gender = customer.Gender,
                DateOfBirth = customer.DateOfBirth,
                CityId = customer.CityId
            };

            if (customer.ImageKey == null)
            {
                searchResult.Results.Add(customerDto);
                continue;
            }

            var url = await GetImageUrlAsync(customer.ImageKey.Value);
            customerDto.ImageUrl = url;

            searchResult.Results.Add(customerDto);
        }

        return searchResult;
    }

    private async Task<string> GetImageUrlAsync(Guid customerImageKey)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "tbc-customer-img",
            Key = customerImageKey.ToString(),
            Expires = DateTime.UtcNow.AddDays(1),

        };
        var url = await amazonS3.GetPreSignedURLAsync(request);

        return url;
    }


    public async Task<int> AddRelation(RelationDTO relation, CancellationToken cancellationToken )
    {
        await EnsureCustomerExistsAsync(relation.CustomerId, cancellationToken);
        await EnsureCustomerExistsAsync(relation.RelatedCustomerId, cancellationToken);
        await EnsureRelationDoesNotExistAsync(relation.CustomerId, relation.RelatedCustomerId, relation.Type, cancellationToken);
        var newRelation = new Relation
        {
            IndividualCustomerId = relation.CustomerId,
            RelatedCustomerId = relation.RelatedCustomerId,
            Type = relation.Type
        };
        var relationId = await repo.AddRelation(newRelation, cancellationToken);

        return relationId;
    }

    public async Task DeleteRelation(int relationId, CancellationToken cancellationToken)
    {
        var relationExists = await repo.GetRelationById(relationId, cancellationToken);
        if (relationExists == null) throw new NotFoundException($"Relation with ID: {relationId} not found");
        await repo.DeleteRelation(relationExists, cancellationToken);
    }

    public async Task<List<RelationReport>> GetRelationReport(int customerId, CancellationToken cancellationToken)
    {
        var customer = await repo.GetCustomerFullDetailsById(customerId, cancellationToken);
        if (customer == null) throw new NotFoundException($"Relation with ID: {customerId} not found");
        var relationReport = customer.Relations.GroupBy(r => r.Type)
            .Select(g => new RelationReport
            {
                Type = g.Key,
                Count = g.Count()
            }).ToList();

        return relationReport;
    }


    // Helper Methods might move to a different class
    private async Task EnsureCustomerExistsAsync(int customerId, CancellationToken cancellationToken)
    {
        var customer = await repo.GetCustomerById(customerId, cancellationToken);
        if (customer == null) throw new NotFoundException($"Customer with ID: {customerId} not found");
    }

    private async Task EnsureRelationDoesNotExistAsync(int customerId, int relatedCustomerId, RelationType type, CancellationToken cancellationToken)
    {
        var exists = await repo.RelationExists(customerId, relatedCustomerId, type, cancellationToken);
        if (exists)
            throw new DuplicationException(
                $"Relation between {customerId} and {relatedCustomerId} with type {type} already exists");
    }

    private List<PhoneNumber> ConvertPhoneNumbers(List<PhoneNumberDTO> phoneNumbers)
    {
        return phoneNumbers.Select(dto => new PhoneNumber
        {
            Number = dto.Number,
            Type = dto.Type,
        }).ToList();
    }

    private void UpdatePhoneNumbers(List<PhoneNumber> existingNumbers, List<UpdatePhoneNumberDTO> incomingNumbers)
    {
        foreach (var phoneNumber in incomingNumbers)
        {
            var phoneNumberFromDb = existingNumbers.FirstOrDefault(p => p.Id == phoneNumber.Id);

            if (phoneNumberFromDb == null)
                throw new NotFoundException($"Phone number with ID: {phoneNumber.Id} not found");
            if (!string.IsNullOrWhiteSpace(phoneNumber.Number)) phoneNumberFromDb.Number = phoneNumber.Number;
            if (phoneNumber.Type.HasValue) phoneNumberFromDb.Type = phoneNumber.Type.Value;
        }
    }
}
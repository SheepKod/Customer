using Amazon.S3;
using Amazon.S3.Model;
using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Application.DTOs;
using Customer.Application.Exceptions;
using Customer.Application.Resources;
using Customer.Domain.Enums;
using Customer.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Customer.Application;

public class CustomerService(ICustomerRepository repo, IAmazonS3 amazonS3) : ICustomerService
{
    public async Task<int> AddCustomer(AddCustomerDTO customer)
    {
        var convertedPhoneNumbers = ConvertPhoneNumbers(customer.PhoneNumbers);
        var city = await repo.GetCityById(customer.CityId);
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
        var customerId = await repo.AddCustomer(newCustomer);

        return customerId;
    }

    public async Task UpdateCustomer(UpdateCustomerDTO updatedCustomerData)
    {
        var customer = await repo.GetCustomerFullDetailsById(updatedCustomerData.CustomerId);
        if (customer == null)
            throw new NotFoundException($"Customer with ID: {updatedCustomerData.CustomerId} not found");

        if (updatedCustomerData.FirstName != null) customer.FirstName = updatedCustomerData.FirstName;

        if (updatedCustomerData.PersonalId != null) customer.PersonalId = updatedCustomerData.PersonalId;

        if (updatedCustomerData.LastName != null) customer.LastName = updatedCustomerData.LastName;

        if (updatedCustomerData.Gender.HasValue) customer.Gender = updatedCustomerData.Gender.Value;

        if (updatedCustomerData.DateOfBirth.HasValue) customer.DateOfBirth = updatedCustomerData.DateOfBirth.Value;

        if (updatedCustomerData.CityId.HasValue)
        {
            var city = await repo.GetCityById(customer.CityId);
            if (city == null) throw new NotFoundException($"City with ID: {customer.CityId} not found");
            customer.CityId = updatedCustomerData.CityId.Value;
        }


        if (updatedCustomerData.PhoneNumbers != null && updatedCustomerData.PhoneNumbers.Any())
        {
            UpdatePhoneNumbers(customer.PhoneNumbers, updatedCustomerData.PhoneNumbers);
        }

        await repo.SaveChangesAsync();
    }

    public async Task DeleteCustomer(int customerId)
    {
        var customer = await repo.GetCustomerById(customerId);
        if (customer == null) throw new NotFoundException($"Customer with ID: {customerId} not found");
        await repo.DeleteCustomer(customer);
    }

    public async Task UploadImage(int customerId, IFormFile image)
    {
        // TODO: bucket name unda gavitano samde readonly constanshi
        var customer = await repo.GetCustomerById(customerId);
        if (customer == null) throw new NotFoundException($"Customer with ID: {customerId} not found");
        if (customer.ImageKey != null)
        {
            try
            {
                await amazonS3.DeleteObjectAsync("tbc-customer-img", $"{customer.ImageKey}");
            }
            catch (AmazonS3Exception ex) when (ex.ErrorCode == StatusCodes.Status404NotFound.ToString())
            {
                throw new NotFoundException($"Image with ID : {customer.ImageKey} not found in S3", ex);
            }

            customer.ImageKey = null;
            await repo.SaveChangesAsync();
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
            await amazonS3.PutObjectAsync(request);
        }

        customer.ImageKey = fileKey;
        await repo.SaveChangesAsync();
    }

    public async Task<PagedResult<IndividualCustomerSearchResultDTO>> SearchCustomers(CustomerDetailedSearchDTO search,
        PagingDTO paging)
    {
        // TODO: Optimize for large data
        var customers = await repo.SearchCustomers(search, paging);

        var result = new PagedResult<IndividualCustomerSearchResultDTO>
        {
            TotalCount = customers.TotalCount,
            PageNumber = customers.PageNumber,
            PageSize = customers.PageSize,
            Results = []
        };
        foreach (var customer in customers.Results)
        {
            var  customerDTO= new IndividualCustomerSearchResultDTO
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
                result.Results.Add(customerDTO);
                continue;
            }
            var request = new GetPreSignedUrlRequest
            {
                BucketName = "tbc-customer-img",
                Key = customer.ImageKey.ToString(),
                Expires = DateTime.UtcNow.AddDays(1)
            };
            var url = await amazonS3.GetPreSignedURLAsync(request);
            customerDTO.ImageUrl = url;
            
            result.Results.Add(customerDTO);
        }
        
        return result;
    }


    public async Task<int> AddRelation(RelationDTO relation)
    {
        await EnsureCustomerExists(relation.CustomerId);
        await EnsureCustomerExists(relation.RelatedCustomerId);
        await EnsureRelationDoesNotExist(relation.CustomerId, relation.RelatedCustomerId, relation.Type);
        var newRelation = new Relation
        {
            IndividualCustomerId = relation.CustomerId,
            RelatedCustomerId = relation.RelatedCustomerId,
            Type = relation.Type
        };
        var relationId = await repo.AddRelation(newRelation);

        return relationId;
    }

    public async Task DeleteRelation(int relationId)
    {
        var relationExists = await repo.GetRelationById(relationId);
        if (relationExists == null) throw new NotFoundException($"Relation with ID: {relationId} not found");
        await repo.DeleteRelation(relationExists);
    }

    public async Task<List<RelationReport>> GetRelationReport(int customerId)
    {
        var customer = await repo.GetCustomerFullDetailsById(customerId);
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
    private async Task EnsureCustomerExists(int customerId)
    {
        var customer = await repo.GetCustomerById(customerId);
        if (customer == null) throw new NotFoundException($"Customer with ID: {customerId} not found");
    }

    private async Task EnsureRelationDoesNotExist(int customerId, int relatedCustomerId, RelationType type)
    {
        var exists = await repo.RelationExists(customerId, relatedCustomerId, type);
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
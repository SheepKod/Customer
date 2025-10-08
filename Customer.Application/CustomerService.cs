using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Application.Exceptions;
using Customer.Domain.Enums;
using Customer.Domain.Models;

namespace Customer.Application;

public class CustomerService(ICustomerRepository repo)
{
    public async Task<int> AddCustomer(AddCustomerDTO customer)
    { 
        var convertedPhoneNumbers = ConvertPhoneNumbers(customer.PhoneNumbers);
        
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
        
        if (customer == null) throw new NotFoundException($"Customer with ID: {updatedCustomerData.CustomerId} not found");
        
        if(updatedCustomerData.FirstName != null) customer.FirstName = updatedCustomerData.FirstName;
        
        if (updatedCustomerData.PersonalId != null) customer.PersonalId = updatedCustomerData.PersonalId;
        
        if(updatedCustomerData.LastName != null) customer.LastName = updatedCustomerData.LastName;
        
        if(updatedCustomerData.Gender.HasValue) customer.Gender = updatedCustomerData.Gender.Value;
        
        if (updatedCustomerData.DateOfBirth.HasValue) customer.DateOfBirth = updatedCustomerData.DateOfBirth.Value;
        
        if(updatedCustomerData.CityId.HasValue) customer.CityId = updatedCustomerData.CityId.Value;
        
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

    public async Task<PagedResult<IndividualCustomer>> QuickSearchCustomers(CustomerQuickSearchDTO search, PagingDTO paging)
    {
        return await repo.QuickSearchCustomers(search, paging);
    }


    public async Task<IndividualCustomer?> GetCustomerById(int customerId)
    {
        var customer = await repo.GetCustomerById(customerId);
        if (customer == null) throw new NotFoundException($"Customer with ID: {customerId} not found");
        return customer;
    }

    private List<PhoneNumber> ConvertPhoneNumbers(List<PhoneNumberDTO> phoneNumbers)
    {
        return phoneNumbers.Select(dto => new PhoneNumber
        {
            Number = dto.Number,
            Type = dto.Type,
        }).ToList();
    }

    public async Task<int> AddRelation(AddRelationDTO addRelation)
    {
        await EnsureCustomerExists(addRelation.CustomerId);
        await EnsureCustomerExists(addRelation.RelatedCustomerId);
        await EnsureRelationDoesNotExist(addRelation.CustomerId, addRelation.RelatedCustomerId, addRelation.Type);
        var newRelation = new Relation
        {
            CustomerId = addRelation.CustomerId,
            RelatedCustomerId = addRelation.RelatedCustomerId,
            Type = addRelation.Type
        };
        var relationId = await repo.AddRelation(newRelation);
        
        return relationId;
    }
    
    public async Task DeleteRelation(int relationId)
    {
   
       var relationExists =  await repo.GetRelationById(relationId);
       if(relationExists == null) throw new NotFoundException($"Relation with ID: {relationId} not found");
       await repo.DeleteRelation(relationExists);
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
            throw new DuplicationException($"Relation between {customerId} and {relatedCustomerId} with type {type} already exists");
    }
    
    private async Task EnsureRelationExist(int customerId, int relatedCustomerId, RelationType type)
    {
        var exists = await repo.RelationExists(customerId, relatedCustomerId, type);
        if (!exists) 
            throw new NotFoundException($"Relation between {customerId} and {relatedCustomerId} with type {type} does not exists");
    }

    private void UpdatePhoneNumbers(List<PhoneNumber> existingNumbers, List<PhoneNumber> incomingNumbers)
    {
        foreach (var phoneNumber in incomingNumbers)
        {
            var phoneNumberExists = existingNumbers.FirstOrDefault(p => p.Id == phoneNumber.Id);
                
            // aq tu ar arsebobs es nomeri mivamato rogorc axali tu error davurtya
            // me mgonia ro raxan update xdeba ar unda ematebodes axali nomeri tu id ar gamoayola
            if(phoneNumberExists == null) throw new NotFoundException($"Phone number with ID: {phoneNumber.Id} not found");
            phoneNumberExists.Number = phoneNumber.Number;
            phoneNumberExists.Type = phoneNumber.Type;
        }
    }

    
}
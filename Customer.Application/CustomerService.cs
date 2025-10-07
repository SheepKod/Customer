using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Application.Exceptions;
using Customer.Domain.Models;

namespace Customer.Application;

public class CustomerService(ICustomerRepository repo)
{
    public async Task<int> AddCustomer(AddCustomerDTO customer)
    {
        var customerExists = await repo.GetCustomerByPersonalId(customer.PersonalId);
        if (customerExists != null) throw new DuplicationException($"Customer with Personal ID: {customer.PersonalId} already exists");
       
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
    
    public async Task DeleteCustomer(int customerId)
    {
        var customer = await repo.GetCustomerById(customerId);
        if (customer == null) throw new NotFoundException($"Customer with ID: {customerId} not found");
        await repo.DeleteCustomer(customer);
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
    
}
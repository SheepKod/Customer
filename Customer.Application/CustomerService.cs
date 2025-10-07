using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Application.Exceptions;
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

    private List<PhoneNumber> ConvertPhoneNumbers(List<PhoneNumberDTO> phoneNumbers)
    {
        return phoneNumbers.Select(dto => new PhoneNumber
        {
            Number = dto.Number,
            Type = dto.Type,
        }).ToList();
    }
    
}
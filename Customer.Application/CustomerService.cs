using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Application.Exceptions;
using Customer.Domain.Models;

namespace Customer.Application;

public class CustomerService(ICustomerRepository repo)
{
    public async Task<IndividualCustomer> AddCustomer(AddCustomerDTO customer)
    {
       var newCustomer = new IndividualCustomer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Gender = customer.Gender,
            PersonalId = customer.PersonalId,
            DateOfBirth = customer.DateOfBirth,
            CityId = customer.CityId,
            PhoneNumbers = customer.PhoneNumbers,
        };
       var dbResult = await repo.AddCustomer(newCustomer);
       if (dbResult == null)
       {
           throw new CustomerCreationFailedException("There was a problem saving the customer in Database.");
       }
      
        return dbResult;
       
    }
}
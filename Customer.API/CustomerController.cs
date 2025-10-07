using Customer.Application;
using Customer.Application.Dtos;
using Customer.Application.Exceptions;
using Customer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API;
[ApiController]
[Route("api/v1/Customer")]
public class CustomerController(CustomerService customerService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(IndividualCustomer), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [ProducesResponseType(503)]
    public async Task<ActionResult<int>> AddCustomer(AddCustomerDTO customer)
    {

        try
        {
            var res = await customerService.AddCustomer(customer);
            return Ok(res);
        }
        catch (CustomerCreationFailedException ex)
        {
            return StatusCode(503, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        
    }
}

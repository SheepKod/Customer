using Customer.Application;
using Customer.Application.Dtos;
using Customer.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API;
[ApiController]
[Route("api/v1/[controller]")]
public class CustomersController(CustomerService customerService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<int>> AddCustomer(AddCustomerDTO customer)
    {
        var customerId = await customerService.AddCustomer(customer);
        return StatusCode(201, customerId);
    }
    
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> DeleteCustomer(int customerId)
    {
        try
        {

            await customerService.DeleteCustomer(customerId);
            return StatusCode(204);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

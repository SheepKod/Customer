using Customer.Application;
using Customer.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API;
[ApiController]
[Route("api/v1/Customer")]
public class CustomerController(CustomerService customerService): ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<int>> AddCustomer(AddCustomerDTO customer)
    {
        var customerId = await customerService.AddCustomer(customer);
        return Ok(customerId);
    }
}

using Customer.Application;
using Customer.Application.Dtos;
using Customer.Application.DTOs;
using Customer.Application.Exceptions;
using Customer.Domain.Models;
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
    public async Task<ActionResult<int>> AddCustomer([FromBody] AddCustomerDTO customer)
    {
        var customerId = await customerService.AddCustomer(customer);
        return StatusCode(201, customerId);
    }
    
    [HttpDelete("{Id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> DeleteCustomer([FromRoute] int customerId)
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
    
    [HttpPatch]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> UpdateCustomer([FromBody] UpdateCustomerDTO updatedCustomerData)
    {
        try
        {
            await customerService.UpdateCustomer(updatedCustomerData);
            return StatusCode(204);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("{Id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetCustomerById([FromRoute] int customerId)
    {
        try
        {
            var customer = await customerService.GetCustomerById(customerId);
            return Ok(customer);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPost("Search")]
    [ProducesResponseType(typeof(PagedResult<IndividualCustomer>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PagedResult<IndividualCustomer>>> QuickSearch([FromBody] CustomerDetailedSearchDTO customerDto, [FromQuery] PagingDTO pagingDto)
    {
      
        var results = await customerService.SearchCustomers
                (customerDto, pagingDto);
        return Ok(results);
       
    }

    [HttpPost("Relations")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> AddRelation([FromBody] AddRelationDTO addRelation)
    {
        try
        {
            var relationId = await customerService.AddRelation(addRelation);
            return StatusCode(201,relationId);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (DuplicationException ex)
        {
            return Conflict(ex.Message);
        }
    }
    
    [HttpDelete("Relations/{Id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteRelation([FromRoute] int id)
    {
        try
        {
             await customerService.DeleteRelation(id);
            return StatusCode(204);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{Id}/Relations")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<RelationReport>>> GetRelationReport([FromRoute] int customerId)
    {
        try
        {
            var report = await customerService.GetRelationReport(customerId);
            return StatusCode(200, report);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

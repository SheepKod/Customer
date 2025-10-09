using Amazon.S3;
using Customer.Application;
using Customer.Application.Dtos;
using Customer.Application.DTOs;
using Customer.Application.Exceptions;
using Customer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API;

[ApiController]
[Route("api/v1/[controller]")]
public class CustomersController(CustomerService customerService) : ControllerBase
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
        await customerService.DeleteCustomer(customerId);
        return StatusCode(204);
    }

    [HttpPatch]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> UpdateCustomer([FromBody] UpdateCustomerDTO updatedCustomerData)
    {
        await customerService.UpdateCustomer(updatedCustomerData);
        return StatusCode(204);
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult> GetCustomerById([FromRoute] int customerId)
    {
        var customer = await customerService.GetCustomerById(customerId);
        return Ok(customer);
    }

    [HttpPost("Search")]
    [ProducesResponseType(typeof(PagedResult<IndividualCustomer>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PagedResult<IndividualCustomer>>> QuickSearch(
        [FromBody] CustomerDetailedSearchDTO customerDto, [FromQuery] PagingDTO pagingDto)
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
            return StatusCode(201, relationId);
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
        await customerService.DeleteRelation(id);
        return StatusCode(204);
    }

    [HttpGet("{Id}/Relations")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<RelationReport>>> GetRelationReport([FromRoute] int customerId)
    {
        var report = await customerService.GetRelationReport(customerId);
        return StatusCode(200, report);
    }

    [HttpPost("{id}/Upload")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UploadImage([FromRoute] int id, [FromForm] IFormFile image)
    {
        try
        {

            await customerService.UploadImage(id, image);

            return Created();
        }
        catch (AmazonS3Exception ex) when(ex.ErrorCode == StatusCodes.Status404NotFound.ToString())
        {
            return NotFound(ex.Message);
        }
    }
}
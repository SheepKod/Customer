using Amazon.S3;
using Customer.Application.Abstractions;
using Customer.Application.Dtos;
using Customer.Application.DTOs;
using Customer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API;

[ApiController]
[ProducesResponseType(400)]
[ProducesResponseType(500)]
[Route("api/v1/[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<int>> AddCustomer([FromBody] AddCustomerDTO customer)
    {
        var customerId = await customerService.AddCustomer(customer);
        return StatusCode(StatusCodes.Status201Created, customerId);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> DeleteCustomer([FromRoute] int id)
    {
        await customerService.DeleteCustomer(id);
        return NoContent();
    }

    [HttpPatch]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> UpdateCustomer([FromBody] UpdateCustomerDTO updatedCustomerData)
    {
        await customerService.UpdateCustomer(updatedCustomerData);
        return NoContent();
    }

    [HttpPost("Search")]
    [ProducesResponseType(typeof(PagedResult<IndividualCustomer>), 200)]
    public async Task<ActionResult<PagedResult<IndividualCustomer>>> QuickSearch(
        [FromBody] CustomerDetailedSearchDTO customerDto, [FromQuery] PagingDTO pagingDto)
    {
        var results = await customerService.SearchCustomers
            (customerDto, pagingDto);
        return Ok(results);
    }

    [HttpPost("Relations")]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<ActionResult<int>> AddRelation([FromBody] AddRelationDTO addRelation)
    {
        var relationId = await customerService.AddRelation(addRelation);
        return StatusCode(StatusCodes.Status201Created, relationId);
    }

    [HttpDelete("Relations/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<ActionResult> DeleteRelation([FromRoute] int id)
    {
        await customerService.DeleteRelation(id);
        return NoContent();
    }

    [HttpGet("{id}/Relations")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<RelationReport>>> GetRelationReport([FromRoute] int id)
    {
        var report = await customerService.GetRelationReport(id);
        return Ok(report);
    }

    [HttpPost("Image")]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> UploadImage([FromForm] UploadImageDto uploadImageDto)
    {
        await customerService.UploadImage(uploadImageDto.CustomerId, uploadImageDto.Image);

        return Created();
    }
}
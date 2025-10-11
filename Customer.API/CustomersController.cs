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
    public async Task<ActionResult<int>> AddCustomer([FromBody] AddCustomerDTO customer, CancellationToken cancellationToken)
    {
        var customerId = await customerService.AddCustomer(customer, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, customerId);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> DeleteCustomer([FromRoute] int id, CancellationToken cancellationToken)
    {
        await customerService.DeleteCustomer(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> UpdateCustomer([FromBody] UpdateCustomerDTO updatedCustomerData, CancellationToken cancellationToken)
    {
        await customerService.UpdateCustomer(updatedCustomerData, cancellationToken);
        return NoContent();
    }

    [HttpPost("Search")]
    [ProducesResponseType(typeof(PagedResult<IndividualCustomer>), 200)]
    public async Task<ActionResult<PagedResult<IndividualCustomer>>> QuickSearch(
        [FromBody] CustomerDetailedSearchDTO customerDto, [FromQuery] PagingDTO pagingDto, CancellationToken cancellationToken)
    {
        var results = await customerService.SearchCustomers
            (customerDto, pagingDto, cancellationToken);
        return Ok(results);
    }

    [HttpPost("Relations")]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<ActionResult<int>> AddRelation([FromBody] RelationDTO relation, CancellationToken cancellationToken)
    {
        var relationId = await customerService.AddRelation(relation, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, relationId);
    }

    [HttpDelete("Relations/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<ActionResult> DeleteRelation([FromRoute] int id, CancellationToken cancellationToken)
    {
        await customerService.DeleteRelation(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id}/Relations")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<List<RelationReport>>> GetRelationReport([FromRoute] int id, CancellationToken cancellationToken)
    {
        var report = await customerService.GetRelationReport(id, cancellationToken);
        return Ok(report);
    }

    [HttpPost("Image")]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> UploadImage([FromForm] UploadImageDto uploadImageDto, CancellationToken cancellationToken)
    {
        await customerService.UploadImage(uploadImageDto.CustomerId, uploadImageDto.Image, cancellationToken);

        return Created();
    }
}
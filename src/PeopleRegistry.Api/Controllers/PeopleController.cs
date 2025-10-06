using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleRegistry.Application.UseCases.Person.Delete;
using PeopleRegistry.Application.UseCases.Person.GetAll;
using PeopleRegistry.Application.UseCases.Person.GetById;
using PeopleRegistry.Application.UseCases.Person.Register;
using PeopleRegistry.Application.UseCases.Person.Update;
using PeopleRegistry.Communication.Requests;
using PeopleRegistry.Communication.Responses;

namespace PeopleRegistry.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/people")] 
[ApiController]
public class PeopleController : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseRegisteredPersonJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] RegisterPersonUseCase useCase,
        [FromBody] RequestRegisterPersonJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }

    [HttpGet("all")]
    [Authorize]
    [ProducesResponseType(typeof(ResponsePeopleJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] GetAllPeopleUseCase useCase)
    {
        var response = await useCase.Execute();
        if (response.People.Any())
        {
            return Ok(response);
        }     
        return NoContent();
    }

    [HttpGet("{id:guid}", Name = "GetPersonById")]
    [Authorize]
    [ProducesResponseType(typeof(ResponsePersonJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] GetPersonByIdUseCase useCase,
        [FromRoute] Guid id)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromServices] UpdatePersonUseCase useCase,
        [FromRoute] Guid id,
        [FromBody] RequestRegisterPersonJson request)
    {
        await useCase.Execute(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] DeletePersonUseCase useCase,
        [FromRoute] Guid id)
    {
        await useCase.Execute(id);
        return NoContent();
    }
}

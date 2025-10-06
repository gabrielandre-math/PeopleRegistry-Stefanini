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

[ApiController]
[Authorize]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/people")]
public class PeopleV2Controller : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredPersonJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] RegisterPersonV2UseCase useCase,
        [FromBody] RequestRegisterPersonV2Json request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }


    [HttpGet]
    [ProducesResponseType(typeof(ResponsePeopleJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] GetAllPeopleUseCase useCase)
    {
        var response = await useCase.Execute();
        return response.People.Any() ? Ok(response) : NoContent();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponsePersonJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] GetPersonByIdUseCase useCase, Guid id)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromServices] UpdatePersonUseCase useCase,
        Guid id,
        [FromBody] RequestRegisterPersonV2Json request)
    {
        
        var v1 = new RequestRegisterPersonJson
        {
            Name = request.Name,
            Cpf = request.Cpf,
            DateOfBirth = request.DateOfBirth,
            Email = request.Email,
            Gender = request.Gender,
            PlaceOfBirth = request.PlaceOfBirth,
            Nationality = request.Nationality
        };

        await useCase.Execute(id, v1);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] DeletePersonUseCase useCase, Guid id)
    {
        await useCase.Execute(id);
        return NoContent();
    }
}

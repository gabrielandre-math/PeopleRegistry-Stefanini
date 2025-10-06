using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleRegistry.Application.UseCases.User.Login;
using PeopleRegistry.Application.UseCases.User.Register;
using PeopleRegistry.Communication.Requests;

namespace PeopleRegistry.Api.Controllers;
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[ApiController]
public class UserController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    [ProducesResponseType(typeof(Communication.Responses.ResponseAuthJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(
    [FromServices] RegisterUserUseCase useCase,
    [FromBody] RequestRegisterUserJson request)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ua = Request.Headers.UserAgent.ToString();
        var response = await useCase.Execute(request, ip, ua);
        return Ok(response);
    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(Communication.Responses.ResponseAuthJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
    [FromServices] LoginUseCase useCase,
    [FromBody] RequestLoginJson request)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ua = Request.Headers.UserAgent.ToString();
        var response = await useCase.Execute(request, ip, ua);
        return Ok(response);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(Communication.Responses.ResponseAuthJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
    [FromServices] RefreshTokenUseCase useCase,
    [FromBody] RequestRefreshTokenJson request)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ua = Request.Headers.UserAgent.ToString();
        var response = await useCase.Execute(request.UserId, request.RefreshToken, ip, ua);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(
    [FromServices] LogoutUseCase useCase,
    [FromBody] RequestLogoutJson request)
    {
        var sid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
        if (!Guid.TryParse(sid, out var userId)) return Unauthorized();
        await useCase.Execute(userId, request.RefreshToken);
        return NoContent();
    }
}

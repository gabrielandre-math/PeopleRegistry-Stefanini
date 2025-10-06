using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleRegistry.Application.UseCases.User.Login;
using PeopleRegistry.Application.UseCases.User.Register;
using PeopleRegistry.Communication.Requests;
using PeopleRegistry.Communication.Responses;

namespace PeopleRegistry.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly RegisterUserUseCase _register;
    private readonly LoginUseCase _login;
    private readonly RefreshTokenUseCase _refresh;
    private readonly LogoutUseCase _logout;

    public AuthController(
        RegisterUserUseCase register,
        LoginUseCase login,
        RefreshTokenUseCase refresh,
        LogoutUseCase logout)
    {
        _register = register;
        _login = login;
        _refresh = refresh;
        _logout = logout;
    }
    /* 
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ResponseAuthJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUserJson request, CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ua = Request.Headers.UserAgent.ToString();
        var response = await _register.Execute(request, ip, ua);
        return Ok(response);
    }
    */
    
    [AllowAnonymous]
    [HttpPost("login")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ResponseAuthJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] RequestLoginJson request, CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ua = Request.Headers.UserAgent.ToString();
        var response = await _login.Execute(request, ip, ua);
        return Ok(response);
    }

    
    [AllowAnonymous]
    [HttpPost("refresh")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ResponseAuthJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RequestRefreshTokenJson request, CancellationToken ct)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var ua = Request.Headers.UserAgent.ToString();
        var response = await _refresh.Execute(request.UserId, request.RefreshToken, ip, ua);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] RequestLogoutJson request, CancellationToken ct)
    {
        var sid = User.FindFirst(System.Security.Claims.ClaimTypes.Sid)?.Value;
        if (!Guid.TryParse(sid, out var userId)) return Unauthorized();

        await _logout.Execute(userId, request.RefreshToken);
        return NoContent();
    }
}

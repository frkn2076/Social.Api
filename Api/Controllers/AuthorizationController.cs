using Api.Infra;
using Api.Service.Contracts;
using Api.ViewModels.Request;
using Api.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly ILogger<AuthorizationController> _logger;
    private readonly IAuthenticationService _authenticationService;

    public AuthorizationController(ILogger<AuthorizationController> logger, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok($"{nameof(AuthorizationController)} works properly!");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestModel request)
    {
        var response = await _authenticationService.Register(request.UserName, request.Password);
        return Ok(response.Response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(RegisterRequestModel request)
    {
        var response = await _authenticationService.Login(request.UserName, request.Password);
        return Ok(response.Response);
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> Refresh(RegisterRequestModel request)
    {
        var refreshToken = HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "RefreshToken").Value;
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest();
        }

        var response = await _authenticationService.GenerateTokenByRefreshToken(refreshToken);
        return Ok(response.Response);
    }
}

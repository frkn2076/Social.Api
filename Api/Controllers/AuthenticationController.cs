using Api.Extensions;
using Api.Filters;
using Api.Infra;
using Api.Service.Contracts;
using Api.Utils.Constants;
using Api.ViewModels.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ExtendedControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly CurrentUser _currentUser;


    public AuthenticationController(ILogger<AuthenticationController> logger,
                                    IAuthenticationService authenticationService,
                                    CurrentUser currentUser)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _currentUser = currentUser;
    }

    [HttpGet]
    public IActionResult Test()
    {
        return Ok($"{nameof(AuthenticationController)} works properly!");
    }

    [HttpPost("register"), CookieSetFilter]
    public async Task<IActionResult> Register(RegisterRequestModel request)
    {
        var response = await _authenticationService.RegisterAsync(request.UserName, request.Password);
        return HandleServiceResponse(response);
    }

    [HttpPost("login"), CookieSetFilter]
    public async Task<IActionResult> Login(RegisterRequestModel request)
    {
        var response = await _authenticationService.LoginAsync(request.UserName, request.Password);
        return HandleServiceResponse(response);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("refresh"), CookieSetFilter]
    public async Task<IActionResult> Refresh()
    {
        var isRefreshToken = HttpContext.User.IsRefreshToken();
        if (!isRefreshToken)
        {
            return Unauthorized();
        }

        var userId = _currentUser.GetId();
        var response = await _authenticationService.LoginByIdAsync(userId);
        return HandleServiceResponse(response);
    }
}

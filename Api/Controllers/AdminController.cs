using Api.Data.Contracts;
using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.Enums;
using Api.Infra;
using Api.Utils.Constants;
using Api.Utils.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = Roles.Admin)]
public class AdminController : ExtendedControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly ISocialRepository _socialRepository;
    private readonly AdminCredentials _adminCredentials;

    public AdminController(ILogger<AdminController> logger, ISocialRepository socialRepository, IOptions<AdminCredentials> adminCredentails)
    {
        _logger = logger;
        _socialRepository = socialRepository;
        _adminCredentials = adminCredentails.Value;
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
        //await TempAdminCredentialsRegisterAsync();
        return Ok($"{nameof(AdminController)} works properly!");
    }

    [HttpGet("test")]
    public async Task<IActionResult> Testtttt()
    {
        return Ok("Admin role is working properly!");
    }

    #region Helper

    

    #endregion Helper
}

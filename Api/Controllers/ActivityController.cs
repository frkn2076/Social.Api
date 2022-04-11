using Api.Data.Contracts;
using Api.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ActivityController : ControllerBase
{
    private readonly ILogger<ActivityController> _logger;
    private readonly ISocialRepository _socialRepository;
    private readonly CurrentUser _currentUser;

    public ActivityController(ILogger<ActivityController> logger, ISocialRepository socialRepository, CurrentUser currentUser)
    {
        _logger = logger;
        _socialRepository = socialRepository;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok($"{nameof(ActivityController)} works properly!");
    }
    
    [HttpGet("claimtest")]
    public async Task<IActionResult> ClaimTest()
    {

        return Ok($"{nameof(ActivityController)} works properly!");
    }
}

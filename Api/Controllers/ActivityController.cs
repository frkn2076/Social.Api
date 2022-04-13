using Api.Data.Contracts;
using Api.Data.Repositories.Contracts;
using Api.Infra;
using Api.Utils.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ActivityController : ExtendedControllerBase
{
    private const int ACTIVITY_PAGINATION_COUNT = 10;

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
    public IActionResult Test()
    {
        return Ok($"{nameof(ActivityController)} works properly!");
    }
    
    [HttpGet("all/{isRefresh?}")]
    public async Task<IActionResult> GetActivities(bool isRefresh)
    {
        var skip = HttpContext.Session.GetInt32(SessionItems.ActivitySkipKey) ?? 0;

        var activities = await _socialRepository.GetActivityAsync(ACTIVITY_PAGINATION_COUNT, skip);

        skip += ACTIVITY_PAGINATION_COUNT;

        HttpContext.Session.SetInt32(SessionItems.ActivitySkipKey, skip);

        return Ok();
    }

    [HttpGet("private/all")]
    public async Task<IActionResult> GetPrivateActivities()
    {
        var skip = HttpContext.Session.GetInt32(SessionItems.ActivitySkipKey) ?? 0;

        var activities = await _socialRepository.GetActivityAsync(ACTIVITY_PAGINATION_COUNT, skip);

        skip += ACTIVITY_PAGINATION_COUNT;

        HttpContext.Session.SetInt32(SessionItems.ActivitySkipKey, skip);

        return Ok();
    }
}

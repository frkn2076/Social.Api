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
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ActivityController : ExtendedControllerBase
{
    private const int ACTIVITY_PAGINATION_COUNT = 10;

    private readonly ILogger<ActivityController> _logger;
    private readonly IActivityService _activityService;
    private readonly CurrentUser _currentUser;

    public ActivityController(ILogger<ActivityController> logger, IActivityService activityService, CurrentUser currentUser)
    {
        _logger = logger;
        _activityService = activityService;
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
        var skip = isRefresh ? 0 : HttpContext.Session.GetInt32(SessionItems.ActivitySkipKey) ?? 0;

        var response = await _activityService.GetActivitiesAsync(ACTIVITY_PAGINATION_COUNT, skip);

        skip += ACTIVITY_PAGINATION_COUNT;

        HttpContext.Session.SetInt32(SessionItems.ActivitySkipKey, skip);

        return HandleServiceResponse(response);
    }

    [HttpGet("private/all")]
    public async Task<IActionResult> GetPrivateActivities()
    {
        var userId = _currentUser.GetId();

        var response = await _activityService.GetUserActivitiesAsync(userId);

        return HandleServiceResponse(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivityDetail(int id)
    {
        var response = await _activityService.GetActivityDetail(id);

        return HandleServiceResponse(response);
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinActivity(JoinActivityRequestModel request)
    {
        var userId = _currentUser.GetId();

        var response = await _activityService.JoinActivityAsync(userId, request.ActivityId);

        return HandleServiceResponse(response);
    }
}

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
    private const int RANDOM_ACTIVITY_COUNT = 5;

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

    [HttpGet("all/random")]
    public async Task<IActionResult> GetActivitiesRandomly()
    {
        var response = await _activityService.GetActivitiesRandomlyAsync(RANDOM_ACTIVITY_COUNT);

        return HandleServiceResponse(response);
    }

    [HttpGet("all/random/search")]
    public async Task<IActionResult> GetActivitiesRandomlyByKeyAsync(string key)
    {
        var response = await _activityService.GetActivitiesRandomlyByTextAsync(RANDOM_ACTIVITY_COUNT, key);

        return HandleServiceResponse(response);
    }

    [HttpGet("private/all")]
    public async Task<IActionResult> GetPrivateActivities()
    {
        var userId = _currentUser.GetId();

        var response = await _activityService.GetUserActivitiesAsync(userId);

        return HandleServiceResponse(response);
    }

    [HttpGet("joined/{userid}")]
    public async Task<IActionResult> GetJoinedActivities(int userId)
    {
        var response = await _activityService.GetUserActivitiesAsync(userId);

        return HandleServiceResponse(response);
    }

    [HttpGet("owner/{id}")]
    public async Task<IActionResult> GetOwnerActivities(int id)
    {
        var response = await _activityService.GetOwnerActivitiesAsync(id);

        return HandleServiceResponse(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivityDetail(int id)
    {
        var userId = _currentUser.GetId();

        var response = await _activityService.GetActivityDetail(id, userId);

        return HandleServiceResponse(response);
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinActivity(JoinActivityRequestModel request)
    {
        var userId = _currentUser.GetId();

        var response = await _activityService.JoinActivityAsync(userId, request.ActivityId);

        return HandleServiceResponse(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(CreateActivityRequestModel request)
    {
        var userId = _currentUser.GetId();

        var response = await _activityService.CreateActivityAsync(request.Title, request.Detail, request.Location, request.Date, request.PhoneNumber, request.Capacity, userId);

        return HandleServiceResponse(response);
    }

    [HttpPost("all/random/filter")]
    public async Task<IActionResult> FilterActivityAsync(ActivityFilterRequestModel request)
    {
        var response = await _activityService.GetActivitiesRandomlyByFilterAsync(RANDOM_ACTIVITY_COUNT, request.FromDate, request.ToDate, request.FromCapacity, request.ToCapacity);

        return HandleServiceResponse(response);
    }
}

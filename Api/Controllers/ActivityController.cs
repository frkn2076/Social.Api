using Api.Data.NoSql.Contracts;
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

    [HttpPost("pagination")]
    public async Task<IActionResult> GetActivities(ActivityPaginationFilterRequestModel request)
    {
        var skip = isFilterChanged(request) ? 0 : HttpContext.Session.GetInt32(SessionItems.ActivitySkipKey) ?? 0;

        var response = await _activityService.GetActivitiesByFilterPaginationAsync(skip, ACTIVITY_PAGINATION_COUNT, request.Key,
            request.FromDate, request.ToDate, request.FromCapacity, request.ToCapacity, request.Categories);

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

        var response = await _activityService.CreateActivityAsync(request.Title, request.Detail, request.Location, request.Date, request.PhoneNumber, request.Capacity, request.Category, userId);

        return HandleServiceResponse(response);
    }

    [HttpPost("all/random/filter")]
    public async Task<IActionResult> FilterActivityAsync(ActivityFilterRequestModel request)
    {
        var response = await _activityService.GetActivitiesRandomlyByFilterAsync(RANDOM_ACTIVITY_COUNT, request.Key, request.FromDate, request.ToDate, request.FromCapacity, request.ToCapacity, request.Categories);

        return HandleServiceResponse(response);
    }


    [HttpGet("messages/{roomid}")]
    public async Task<IActionResult> GetRoomMessages(int roomId)
    {
        var response = await _activityService.GetRoomMessages(roomId);

        return HandleServiceResponse(response);
    }

    #region Helper

    private bool isFilterChanged(ActivityPaginationFilterRequestModel request)
    {
        var key = HttpContext.Session.GetString(SessionItems.SearchKeyFilterKey);
        var fromDate = HttpContext.Session.GetString(SessionItems.FromDateFilterKey);
        var toDate = HttpContext.Session.GetString(SessionItems.ToDateFilterKey);
        var fromCapacity = HttpContext.Session.GetInt32(SessionItems.FromCapacityFilterKey);
        var toCapacity = HttpContext.Session.GetInt32(SessionItems.ToCapacityFilterKey);
        var categories = HttpContext.Session.GetString(SessionItems.CategoriesFilterKey);

        var isFilterChanged = request.IsRefresh
            || key is not null && (key ?? string.Empty) != (request.Key ?? string.Empty)
            || fromCapacity is not null && fromCapacity != request.FromCapacity
            || toCapacity is not null && toCapacity != request.ToCapacity
            || fromDate is not null &&  fromDate != request.FromDate.ToString()
            || toDate is not null && toDate != request.ToDate.ToString()
            || categories is not null && categories != string.Join(',', request.Categories);

        HttpContext.Session.SetString(SessionItems.SearchKeyFilterKey, request.Key ?? string.Empty);
        HttpContext.Session.SetString(SessionItems.FromDateFilterKey, request.FromDate.ToString());
        HttpContext.Session.SetString(SessionItems.ToDateFilterKey, request.ToDate.ToString());
        HttpContext.Session.SetInt32(SessionItems.FromCapacityFilterKey, request.FromCapacity);
        HttpContext.Session.SetInt32(SessionItems.ToCapacityFilterKey, request.ToCapacity);
        HttpContext.Session.SetString(SessionItems.CategoriesFilterKey, string.Join(',', request.Categories));
        
        return isFilterChanged;
    }

    #endregion
}

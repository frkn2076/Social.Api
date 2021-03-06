using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.Infra;
using Api.Service.Contracts;
using Api.Utils.Constants;
using Api.ViewModels.Response;

namespace Api.Service.Implementations;

public class ActivityService : IActivityService
{
    private readonly ISocialRepository _socialRepository;

    public ActivityService(ISocialRepository socialRepository)
    {
        _socialRepository = socialRepository;
    }

    public async Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesAsync(int skip, int count)
    {
        var activities = await _socialRepository.GetActivityAsync(count, skip);

        if(activities == null || !activities.Any())
        {
            return new()
            {
                Error = ErrorMessages.NoRecordHasFound
            };
        }

        return new()
        {
            IsSuccessful = true,
            Response = activities
        };
    }

    public async Task<ServiceResponse<IEnumerable<Activity>>> GetUserActivitiesAsync(int userId)
    {
        var activities = await _socialRepository.GetUserActivityAsync(userId);

        if (activities == null || !activities.Any())
        {
            return new()
            {
                Error = ErrorMessages.NoRecordHasFound
            };
        }

        return new()
        {
            IsSuccessful = true,
            Response = activities
        };
    }

    public async Task<ServiceResponse<ActivityDetailResponseModel>> GetActivityDetail(int activityId)
    {
        var activity = await _socialRepository.GetActivityByIdAsync(activityId);

        if (activity == null)
        {
            return new()
            {
                Error = ErrorMessages.NoRecordHasFound
            };
        }

        var joiners = await _socialRepository.GetUsersByActivityIdQueryAsync(activityId);

        joiners ??= new List<Profile>();

        var response = new ActivityDetailResponseModel()
        {
            Id = activityId,
            Date = activity.Date,
            Detail = activity.Detail,
            Location = activity.Location,
            Title = activity.Title,
            Joiners = joiners.Select(x => new UserResponseModel()
            {
                Id = x.Id,
                UserName = x.UserName
            }).ToList()
        };

        return new()
        {
            IsSuccessful = true,
            Response = response
        };
    }
}

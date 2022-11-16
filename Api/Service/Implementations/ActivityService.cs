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

    public async Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesAsync(int count, int skip)
    {
        var activities = await _socialRepository.GetActivityAsync(count, skip);

        if (!activities?.Any() ?? true)
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

    public async Task<ServiceResponse<IEnumerable<Activity>>> GetOwnerActivitiesAsync(int userId)
    {
        var activities = await _socialRepository.GetOwnerActivityAsync(userId);

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

    public async Task<ServiceResponse<ActivityDetailResponseModel>> GetActivityDetail(int activityId, int userId)
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
            UserId = userId,
            Detail = activity.Detail,
            Location = activity.Location,
            Title = activity.Title,
            PhoneNumber = activity.PhoneNumber,
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

    public async Task<ServiceResponse<bool>> JoinActivityAsync(int profileId, int activityId)
    {
        try
        {
            await _socialRepository.CreateProfileActivityAsync(activityId, profileId);
            return new()
            {
                IsSuccessful = true,
                Response = true
            };
        }
        catch (Exception ex)
        {
            // log ex
            return new()
            {
                Error = ErrorMessages.OperationHasFailed
            };
        }
    }

    public async Task<ServiceResponse<bool>> CreateActivityAsync(string title, string detail, string location, DateTime? date, string phoneNumber, int userId)
    {
        try
        {
            var activity = new Activity()
            {
                Title = title,
                Detail = detail,
                Location = location,
                Date = date,
                PhoneNumber = phoneNumber,
                OwnerProfileId = userId
            };
            var createdActivity = await _socialRepository.CreateActivityAsync(activity);

            if ((createdActivity?.Id ?? 0) <= 0)
            {
                return new()
                {
                    Error = ErrorMessages.OperationHasFailed
                };
            }

            await _socialRepository.CreateProfileActivityAsync(createdActivity.Id, userId);

            return new()
            {
                IsSuccessful = true,
                Response = true
            };
        }
        catch (Exception ex)
        {
            // log ex
            return new()
            {
                Error = ErrorMessages.OperationHasFailed
            };
        }
    }
}

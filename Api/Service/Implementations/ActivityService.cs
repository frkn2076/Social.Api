using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.Infra;
using Api.Service.Contracts;
using Api.Utils.Constants;

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

    public async Task<ServiceResponse<IEnumerable<Activity>>> GetUserActivitiesAsync(int id)
    {
        var activities = await _socialRepository.GetUserActivityAsync(id);

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
}

using Api.Data.Entities;
using Api.Infra;
using Api.ViewModels.Response;

namespace Api.Service.Contracts;

public interface IActivityService
{
    Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesAsync(int skip, int count);

    Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesRandomlyAsync(int count);

    Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesRandomlyByTextAsync(int count, string key);

    Task<ServiceResponse<IEnumerable<Activity>>> GetUserActivitiesAsync(int userId);

    Task<ServiceResponse<IEnumerable<Activity>>> GetOwnerActivitiesAsync(int userId);

    Task<ServiceResponse<ActivityDetailResponseModel>> GetActivityDetail(int activityId, int userId);

    Task<ServiceResponse<bool>> JoinActivityAsync(int profileId, int activityId);

    Task<ServiceResponse<bool>> CreateActivityAsync(string title, string detail, string location, DateTime? date, string phoneNumber, int capacity, int userId);
}

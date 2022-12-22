using Api.Data.Entities;
using Api.Infra;
using Api.ViewModels.Response;

namespace Api.Service.Contracts;

public interface IActivityService
{
    Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesByFilterPaginationAsync(int skip, int count, string key, DateTime fromDate, DateTime toDate, int fromCapacity, int toCapacity, List<string> categories);

    Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesRandomlyByFilterAsync(int count, string key, DateTime fromDate, DateTime toDate, int fromCapacity, int toCapacity, List<string> categories);

    Task<ServiceResponse<IEnumerable<Activity>>> GetUserActivitiesAsync(int userId);

    Task<ServiceResponse<IEnumerable<Activity>>> GetOwnerActivitiesAsync(int userId);

    Task<ServiceResponse<ActivityDetailResponseModel>> GetActivityDetail(int activityId, int userId);

    Task<ServiceResponse<bool>> JoinActivityAsync(int profileId, int activityId);

    Task<ServiceResponse<bool>> CreateActivityAsync(string title, string detail, string location, DateTime? date, string phoneNumber, int capacity, string category, int userId);

    Task<ServiceResponse<string>> GetRoomMessages(int roomId);
}

using Api.Data.Entities;
using System.Data;

namespace Api.Data.Repositories.Contracts;

public interface ISocialRepository
{
    Task<Profile> CreateProfileAsync(Profile profile, IDbTransaction transaction = null);

    Task<Activity> CreateActivityAsync(Activity activity, IDbTransaction transaction = null);

    Task CreateProfileActivityAsync(int activityId, int profileId, IDbTransaction transaction = null);

    Task<Profile> GetProfileByUserNameAsync(string userName, IDbTransaction transaction = null);

    Task<Profile> GetProfileByIdAsync(int id, IDbTransaction transaction = null);

    Task<bool> UpdateProfileAsync(int id, string name, string photo, string about, IDbTransaction transaction = null);

    Task<IEnumerable<Activity>> GetActivityAsync(int count, int skip, IDbTransaction transaction = null);

    Task<IEnumerable<Activity>> GetActivityRandomlyByFilterAsync(int count, string key, DateTime fromDate, DateTime toDate, int fromCapacity, int toCapacity, List<string> categories, IDbTransaction transaction = null);

    Task<IEnumerable<Activity>> GetUserActivityAsync(int id, IDbTransaction transaction = null);

    Task<IEnumerable<Activity>> GetOwnerActivityAsync(int id, IDbTransaction transaction = null);

    Task<Activity> GetActivityByIdAsync(int activityId, IDbTransaction transaction = null);

    Task<IEnumerable<Profile>> GetUsersByActivityIdQueryAsync(int activityId, IDbTransaction transaction = null);
}

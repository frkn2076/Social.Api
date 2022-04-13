using Api.Data.Entities;
using Api.Infra;

namespace Api.Service.Contracts;

public interface IActivityService
{
    Task<ServiceResponse<IEnumerable<Activity>>> GetActivitiesAsync(int skip, int count);

    Task<ServiceResponse<IEnumerable<Activity>>> GetUserActivitiesAsync(int id);
}

using Api.Data.Entities;
using Api.Infra;

namespace Api.Service.Contracts;

public interface IProfileService
{
    Task<ServiceResponse<Profile>> GetProfileAsync(int id);

    Task<ServiceResponse<bool>> UpdateProfileAsync(int id, string name, string surname, string photo);
}

using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.Infra;
using Api.Service.Contracts;
using Api.Utils.Constants;

namespace Api.Service.Implementations;

public class ProfileService : IProfileService
{
    private readonly ISocialRepository _socialRepository;
    
    public ProfileService(ISocialRepository socialRepository)
    {
        _socialRepository = socialRepository;
    }

    public async Task<ServiceResponse<Profile>> GetProfileAsync(int id)
    {
        var profile = await _socialRepository.GetProfileByIdAsync(id);

        if (profile == null)
        {
            return new()
            {
                Error = ErrorMessages.UserNotFound
            };
        }

        return new()
        {
            IsSuccessful = true,
            Response = profile
        };
    }

    public async Task<ServiceResponse<bool>> UpdateProfileAsync(int id, string name, string photo, string about)
    {
        var isSucceed = await _socialRepository.UpdateProfileAsync(id, name, photo, about);

        if (!isSucceed)
        {
            return new()
            {
                Error = ErrorMessages.OperationHasFailed
            };
        }

        return new()
        {
            IsSuccessful = true,
            Response = true
        };
    }
}

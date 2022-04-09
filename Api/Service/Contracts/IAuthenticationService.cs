using Api.Infra;
using Api.ViewModels.Response;

namespace Api.Service.Contracts;

public interface IAuthenticationService
{
    Task<ServiceResponse<AuthenticationResponseModel>> Register(string userName, string userPassword);

    Task<ServiceResponse<AuthenticationResponseModel>> Login(string userName, string userPassword);

    Task<ServiceResponse<AuthenticationResponseModel>> GenerateTokenByRefreshToken(string refreshToken);
}

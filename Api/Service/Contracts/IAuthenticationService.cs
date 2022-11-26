using Api.Infra;
using Api.ViewModels.Response;

namespace Api.Service.Contracts;

public interface IAuthenticationService
{
    Task<ServiceResponse<AuthenticationResponseModel>> RegisterAsync(string userName, string userPassword);

    Task<ServiceResponse<AuthenticationResponseModel>> LoginAsync(string userName, string userPassword);

    Task<ServiceResponse<AuthenticationResponseModel>> LoginByIdAsync(int userId);
}

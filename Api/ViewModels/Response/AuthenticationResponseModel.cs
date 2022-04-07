namespace Api.ViewModels.Response;

public class AuthenticationResponseModel
{
    public string AccessToken { get; set; }

    public int AccessTokenExpireDate { get; set; }

    public string RefreshToken { get; set; }

    public int RefreshTokenExpireDate { get; set; }
}

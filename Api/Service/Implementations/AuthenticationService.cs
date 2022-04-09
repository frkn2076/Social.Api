using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.Helper;
using Api.Infra;
using Api.Service.Contracts;
using Api.Utils;
using Api.ViewModels.Response;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Service.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly ISocialRepository _socialRepository;
    private readonly string _secretKey;
    private readonly int _accessExpireDate;
    private readonly int _refreshExpireDate;

    public AuthenticationService(ISocialRepository socialRepository)
    {
        _socialRepository = socialRepository;
        _secretKey = "SECRET KEY";
        _accessExpireDate = 60;
        _refreshExpireDate = 6000;
    }

    public async Task<ServiceResponse<AuthenticationResponseModel>> Register(string userName, string userPassword)
    {
        var profile = new Profile()
        {
            UserName = userName,
            Password = userPassword
        };

        var existingUser = await _socialRepository.GetProfileByUserNameAsync(userName);

        if (existingUser != null)
        {
            return new()
            {
                Error = Constants.UserNotFound
            };
        }

        var createdProfile = await _socialRepository.CreateProfileAsync(profile);

        if (createdProfile?.Id == null)
        {
            return new()
            {
                Error = Constants.OperationHasFailed
            };
        }

        return await GenerateTokenByCredentialsAsync(createdProfile);
    }

    public async Task<ServiceResponse<AuthenticationResponseModel>> Login(string userName, string userPassword)
    {
        var user = await _socialRepository.GetProfileByUserNameAsync(userName);

        if (user == null)
        {
            return new()
            {
                Error = Constants.UserNotFound
            };
        }

        var encryptedUserPassword = CryptoHelper.EncryptPassword(userPassword);

        if (encryptedUserPassword != user.Password)
        {
            return new()
            {
                Error = Constants.WrongPassword
            };
        }

        return await GenerateTokenByCredentialsAsync(user);
    }

    public async Task<ServiceResponse<AuthenticationResponseModel>> GenerateTokenByRefreshToken(string refreshToken)
    {
        var user = await _socialRepository.GetProfileByRefreshTokenAsync(refreshToken);

        if (user == null)
        {
            return new()
            {
                Error = Constants.UserNotFound
            };
        }

        if (user.ExpireDate.AddMinutes(_refreshExpireDate) < DateTime.UtcNow)
        {
            return new()
            {
                Error = Constants.TokenExpired
            };
        }

        var token = await GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        await _socialRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMinutes(_refreshExpireDate));

        var response = new AuthenticationResponseModel()
        {
            AccessToken = token,
            AccessTokenExpireDate = _accessExpireDate,
            RefreshToken = newRefreshToken,
            RefreshTokenExpireDate = _refreshExpireDate
        };

        return new()
        {
            IsSuccessful = true,
            Response = response
        };
    }

    #region Helper

    private async Task<ServiceResponse<AuthenticationResponseModel>> GenerateTokenByCredentialsAsync(Profile user)
    {
        var token = await GenerateJwtToken(user);

        var refreshToken = GenerateRefreshToken();

        await _socialRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMinutes(_refreshExpireDate));

        var response = new AuthenticationResponseModel()
        {
            AccessToken = token,
            AccessTokenExpireDate = _accessExpireDate,
            RefreshToken = refreshToken,
            RefreshTokenExpireDate = _refreshExpireDate
        };

        return new()
        {
            IsSuccessful = true,
            Response = response
        };
    }

    private async Task<string> GenerateJwtToken(Profile user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.UserName),
                new Claim("email", user.Email),
            }),
            Expires = DateTime.UtcNow.AddMinutes(_accessExpireDate),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    #endregion Helper
}

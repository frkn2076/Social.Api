using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.Helper;
using Api.Infra;
using Api.Service.Contracts;
using Api.Utils.Constants;
using Api.Utils.Models;
using Api.ViewModels.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Service.Implementations;

public class AuthenticationService : IAuthenticationService
{
    private readonly ISocialRepository _socialRepository;
    private readonly JWTSettings _jwtSettings;

    public AuthenticationService(ISocialRepository socialRepository, IOptions<JWTSettings> jwtSettings)
    {
        _socialRepository = socialRepository;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<ServiceResponse<AuthenticationResponseModel>> Register(string userName, string userPassword)
    {
        var encryptedPassword = CryptoHelper.EncryptPassword(userPassword);

        var profile = new Profile()
        {
            UserName = userName,
            Password = encryptedPassword
        };

        var existingUser = await _socialRepository.GetProfileByUserNameAsync(userName);

        if (existingUser != null)
        {
            return new()
            {
                Error = ErrorMessages.UserNotFound
            };
        }

        var createdProfile = await _socialRepository.CreateProfileAsync(profile);

        if (createdProfile == null)
        {
            return new()
            {
                Error = ErrorMessages.OperationHasFailed
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
                Error = ErrorMessages.UserNotFound
            };
        }

        var encryptedUserPassword = CryptoHelper.EncryptPassword(userPassword);

        if (encryptedUserPassword != user.Password)
        {
            return new()
            {
                Error = ErrorMessages.WrongPassword
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
                Error = ErrorMessages.UserNotFound
            };
        }

        if (user.ExpireDate.AddMinutes(_jwtSettings.RefreshExpireDate) < DateTime.UtcNow)
        {
            return new()
            {
                Error = ErrorMessages.TokenExpired
            };
        }

        var token = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        await _socialRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshExpireDate));

        var response = new AuthenticationResponseModel()
        {
            AccessToken = token,
            AccessTokenExpireDate = _jwtSettings.AccessExpireDate,
            RefreshToken = newRefreshToken,
            RefreshTokenExpireDate = _jwtSettings.RefreshExpireDate
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
        var token = GenerateJwtToken(user);

        var refreshToken = GenerateRefreshToken();

        await _socialRepository.UpdateRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshExpireDate));

        var response = new AuthenticationResponseModel()
        {
            AccessToken = token,
            AccessTokenExpireDate = _jwtSettings.AccessExpireDate,
            RefreshToken = refreshToken,
            RefreshTokenExpireDate = _jwtSettings.RefreshExpireDate
        };

        return new()
        {
            IsSuccessful = true,
            Response = response
        };
    }

    private string GenerateJwtToken(Profile user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimNames.Id, user.Id.ToString())
        };

        if (!string.IsNullOrEmpty(user.UserName))
        {
            claims.Add(new Claim(ClaimNames.UserName, user.UserName));
        }

        if (!string.IsNullOrEmpty(user.Email))
        {
            claims.Add(new Claim(ClaimNames.Email, user.Email));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessExpireDate),
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

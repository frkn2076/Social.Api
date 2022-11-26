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

    public async Task<ServiceResponse<AuthenticationResponseModel>> RegisterAsync(string userName, string userPassword)
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
                Error = ErrorMessages.UserAlreadyExists
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

        return GenerateToken(createdProfile.Id);
    }

    public async Task<ServiceResponse<AuthenticationResponseModel>> LoginAsync(string userName, string userPassword)
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

        return GenerateToken(user.Id);
    }

    public async Task<ServiceResponse<AuthenticationResponseModel>> LoginByIdAsync(int userId)
    {
        var user = await _socialRepository.GetProfileByIdAsync(userId);

        if (user == null)
        {
            return new()
            {
                Error = ErrorMessages.UserNotFound
            };
        }

        return GenerateToken(user.Id);
    }

    #region Helper

    private ServiceResponse<AuthenticationResponseModel> GenerateToken(int userId)
    {
        var token = new AuthenticationResponseModel()
        {
            AccessToken = GenerateJWTToken(userId, false),
            AccessTokenExpireDate = _jwtSettings.AccessExpireDate,
            RefreshToken = GenerateJWTToken(userId, true),
            RefreshTokenExpireDate = _jwtSettings.RefreshExpireDate
        };

        return new()
        {
            IsSuccessful = true,
            Response = token
        };
    }

    private string GenerateJWTToken(int userId, bool isRefreshToken)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.SerialNumber, userId.ToString())
        };

        if (isRefreshToken)
        {
            claims.Add(new Claim(ClaimTypes.AuthorizationDecision, "RefreshToken"));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(isRefreshToken ? _jwtSettings.RefreshExpireDate : _jwtSettings.AccessExpireDate),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    #endregion Helper
}

using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.Infra;
using Api.Utils;
using Api.ViewModels.Response;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Api.Helper;

public class JWTHelper
{
    private readonly ISocialRepository _socialRepository;
    private readonly string _secretKey;
    private readonly int _accessExpireDate;
    private readonly int _refreshExpireDate;

    public JWTHelper(ISocialRepository socialRepository)
    {
        _socialRepository = socialRepository;
        _secretKey = "SECRET KEY";
        _accessExpireDate = 60;
        _refreshExpireDate = 600;
    }

    public async Task<ServiceResponse> GenerateTokenByRefreshToken(string refreshToken)
    {
        var user = await _socialRepository.GetProfileByRefreshTokenAsync(refreshToken);

        if (user == null)
        {
            return new ServiceResponse()
            {
                Error = Constants.UserNotFound
            };
        }

        if(user.ExpireDate.AddMinutes(_refreshExpireDate) < DateTime.Now)
        {
            return new ServiceResponse()
            {
                Error = Constants.TokenExpired
            };
        }

        var token = await GenerateJwtToken(user, _refreshExpireDate);
        var newRefreshToken = GenerateRefreshToken();

        var response = new AuthenticationResponseModel()
        {
            AccessToken = token,
            AccessTokenExpireDate = _accessExpireDate,
            RefreshToken = newRefreshToken,
            RefreshTokenExpireDate = _refreshExpireDate
        };

        return new ServiceResponse<AuthenticationResponseModel>()
        {
            IsSuccessful = true,
            Response = response
        };
    }

    public async Task<ServiceResponse> GenerateTokenByCredentials(string userName, string password)
    {
        var user = await _socialRepository.GetProfileByUserNameAsync(userName);

        if (user == null)
        {
            return new ServiceResponse()
            {
                Error = Constants.UserNotFound
            };
        }

        var encryptedUserPassword = CryptoHelper.EncryptPassword(password);

        if (encryptedUserPassword != user.Password)
        {
            return new ServiceResponse()
            {
                Error = Constants.WrongPassword
            };
        }

        var token = await GenerateJwtToken(user, 60);

        var refreshToken = GenerateRefreshToken();

        var response = new AuthenticationResponseModel()
        {
            AccessToken = token,
            AccessTokenExpireDate = _accessExpireDate,
            RefreshToken = refreshToken,
            RefreshTokenExpireDate = _refreshExpireDate
        };

        return new ServiceResponse<AuthenticationResponseModel>()
        {
            IsSuccessful = true,
            Response = response
        };
    }


    public async Task<string> GenerateJwtToken(Profile user, int expireMinutes)
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
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public int? ValidateJwtToken(string token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            return userId;
        }
        catch
        {
            return null;
        }
    }

    #region Helper

    private string GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[64];
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private async Task<bool> ValidatePassword(string email, string userPassword)
    {
        var password = await _socialRepository.GetPasswordAsync(email);
        var encryptedUserPassword = CryptoHelper.EncryptPassword(userPassword);
        return password == encryptedUserPassword;
    }

    #endregion Helper
}

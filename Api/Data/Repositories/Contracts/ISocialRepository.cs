using Api.Data.Entities;
using System.Data;

namespace Api.Data.Repositories.Contracts;

public interface ISocialRepository
{
    Task CreateProfileAsync(Profile profile, IDbTransaction transaction = null);

    Task<string> GetPasswordAsync(string email, IDbTransaction transaction = null);

    Task<Profile> GetProfileByRefreshTokenAsync(string refreshToken, IDbTransaction transaction = null);

    Task<Profile> GetProfileByUserNameAsync(string userName, IDbTransaction transaction = null);

    Task UpdateRefreshTokenAsync(int id, string refreshToken, DateTime expireDate, IDbTransaction transaction = null);
}

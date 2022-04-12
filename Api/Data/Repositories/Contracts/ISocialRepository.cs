using Api.Data.Entities;
using System.Data;

namespace Api.Data.Repositories.Contracts;

public interface ISocialRepository
{
    Task<Profile> CreateProfileAsync(Profile profile, IDbTransaction transaction = null);

    Task<string> GetPasswordAsync(string email, IDbTransaction transaction = null);

    Task<Profile> GetProfileByRefreshTokenAsync(string refreshToken, IDbTransaction transaction = null);

    Task<Profile> GetProfileByUserNameAsync(string userName, IDbTransaction transaction = null);

    Task<bool> UpdateRefreshTokenAsync(int id, string refreshToken, DateTime expireDate, IDbTransaction transaction = null);

    Task<Profile> GetProfileByIdAsync(int id, IDbTransaction transaction = null);

    Task<bool> UpdateProfileAsync(int id, string name, string surname, string photo, IDbTransaction transaction = null);

    Task<IEnumerable<Activity>> GetActivityAsync(int count, int skip, IDbTransaction transaction = null);
}

using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Dapper;
using Npgsql;
using System.Data;

namespace Api.Data.Repositories.Implementations;

public class SocialRepository : ISocialRepository
{
    private readonly string _connectionString;

    public SocialRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PostgresContext");
    }

    public async Task<Profile> CreateProfileAsync(Profile profile, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Profile>(GetQuery(Constants.Queries.CreateProfileQuery), profile, transaction: transaction);
        }
    }

    public async Task<Activity> CreateActivityAsync(Activity activity, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Activity>(GetQuery(Constants.Queries.CreateActivityQuery), activity, transaction: transaction);
        }
    }

    public async Task CreateProfileActivityAsync(int activityId, int profileId, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            await connection.QueryFirstOrDefaultAsync<Profile>(GetQuery(Constants.Queries.CreateProfileActivityQuery), new { profileId, activityId },
                transaction: transaction);
        }
    }

    public async Task<string> GetPasswordAsync(string email, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstAsync<string>(GetQuery(Constants.Queries.GetPasswordQuery), new { email }, transaction: transaction);
        }
    }

    public async Task<Profile> GetProfileByRefreshTokenAsync(string refreshToken, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Profile>(GetQuery(Constants.Queries.GetProfileByRefreshTokenQuery), new { refreshToken }, transaction: transaction);
        }
    }

    public async Task<Profile> GetProfileByUserNameAsync(string userName, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Profile>(GetQuery(Constants.Queries.GetProfileByUserNameQuery), new { userName }, transaction: transaction);
        }
    }

    public async Task<Profile> GetProfileByIdAsync(int id, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Profile>(GetQuery(Constants.Queries.GetProfileByIdQuery), new { id }, transaction: transaction);
        }
    }

    public async Task<bool> UpdateRefreshTokenAsync(int id, string refreshToken, DateTime expireDate, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var affectedRows = await connection.ExecuteAsync(GetQuery(Constants.Queries.UpdateRefreshTokenQuery), new { id, refreshToken, expireDate },
                transaction: transaction);
            return affectedRows > 0;
        }

    }

    public async Task<bool> UpdateProfileAsync(int id, string name, string surname, string photo, string about, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var affectedRows = await connection.ExecuteAsync(GetQuery(Constants.Queries.UpdateProfileQuery), new { id, name, surname, photo, about },
                transaction: transaction);
            return affectedRows > 0;
        }
    }

    public async Task<IEnumerable<Activity>> GetActivityAsync(int count, int skip, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Activity>(GetQuery(Constants.Queries.GetActivityQuery), new { count, skip }, transaction: transaction);
        }
    }

    public async Task<IEnumerable<Activity>> GetActivityRandomlyAsync(int count, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Activity>(GetQuery(Constants.Queries.GetActivityRandomlyQuery), new { count }, transaction: transaction);
        }
    }

    public async Task<IEnumerable<Activity>> GetActivityRandomlyByKeyAsync(int count, string key, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Activity>(GetQuery(Constants.Queries.GetActivityRandomlyByKeyQuery), new { count, key }, transaction: transaction);
        }
    }

    public async Task<IEnumerable<Activity>> GetActivityRandomlyByFilterAsync(int count, string key, DateTime fromDate, DateTime toDate,
        int fromCapacity, int toCapacity, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Activity>(GetQuery(Constants.Queries.GetActivityRandomlyByFilterQuery),
                new { count, key, fromDate, toDate, fromCapacity, toCapacity }, transaction: transaction);
        }
    }

    public async Task<IEnumerable<Activity>> GetUserActivityAsync(int id, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Activity>(GetQuery(Constants.Queries.GetUserActivityQuery), new { id }, transaction: transaction);
        }
    }

    public async Task<IEnumerable<Activity>> GetOwnerActivityAsync(int id, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Activity>(GetQuery(Constants.Queries.GetOwnerActivityQuery), new { id }, transaction: transaction);
        }
    }

    public async Task<Activity> GetActivityByIdAsync(int activityId, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Activity>(GetQuery(Constants.Queries.GetActivityByIdQuery), new { activityId }, transaction: transaction);
        }
    }

    public async Task<IEnumerable<Profile>> GetUsersByActivityIdQueryAsync(int activityId, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Profile>(GetQuery(Constants.Queries.GetUsersByActivityIdQuery), new { activityId }, transaction: transaction);
        }
    }

    #region Helper

    private string GetQuery(string queryFileName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = Path.Combine(currentDirectory, Constants.AdhocFolderPath);
        var file = string.Concat(queryFileName, Constants.SqlFileExtension);
        var filePath = Path.Combine(path, file);
        return File.ReadAllText(filePath);
    }

    #endregion Helper
}

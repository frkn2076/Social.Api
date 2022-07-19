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
            return await connection.QueryFirstOrDefaultAsync<Profile>(GetQuery(Queries.CreateProfileQuery), profile, transaction: transaction);
        }
    }

    public async Task<string> GetPasswordAsync(string email, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstAsync<string>(GetQuery(Queries.GetPasswordQuery), new { email }, transaction: transaction);
        }
    }

    public async Task<Profile> GetProfileByRefreshTokenAsync(string refreshToken, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Profile>(GetQuery(Queries.GetProfileByRefreshTokenQuery), new { refreshToken }, transaction: transaction);
        }
    }

    public async Task<Profile> GetProfileByUserNameAsync(string userName, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Profile>(GetQuery(Queries.GetProfileByUserNameQuery), new { userName }, transaction: transaction);
        }
    }

    public async Task<Profile> GetProfileByIdAsync(int id, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Profile>(GetQuery(Queries.GetProfileByIdQuery), new { id }, transaction: transaction);
        }
    }

    public async Task<bool> UpdateRefreshTokenAsync(int id, string refreshToken, DateTime expireDate, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var affectedRows = await connection.ExecuteAsync(GetQuery(Queries.UpdateRefreshTokenQuery), new { id, refreshToken, expireDate },
                transaction: transaction);
            return affectedRows > 0;
        }
        
    }

    public async Task<bool> UpdateProfileAsync(int id, string name, string surname, string photo, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var affectedRows = await connection.ExecuteAsync(GetQuery(Queries.UpdateProfileQuery), new { id, name, surname, photo },
                transaction: transaction);
            return affectedRows > 0;
        }
    }

    public async Task<IEnumerable<Activity>> GetActivityAsync(int count, int skip, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Activity>(GetQuery(Queries.GetActivityQuery), new { count, skip }, transaction: transaction);
        }
    }
    
    public async Task<IEnumerable<Activity>> GetUserActivityAsync(int id, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Activity>(GetQuery(Queries.GetUserActivityQuery), new { id }, transaction: transaction);
        }
    }

    public async Task<Activity> GetActivityByIdAsync(int activityId, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<Activity>(GetQuery(Queries.GetActivityQuery), new { activityId }, transaction: transaction);
        }
    }

    public async Task<IEnumerable<Profile>> GetUsersByActivityIdQueryAsync(int activityId, IDbTransaction transaction = null)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<Profile>(GetQuery(Queries.GetUsersByActivityIdQuery), new { activityId }, transaction: transaction);
        }
    }

    #region Helper

    private string GetQuery(string queryFileName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = Path.Combine(currentDirectory, Queries.AdhocFolderPath);
        var file = string.Concat(queryFileName, Queries.SqlFileExtension);
        var filePath = Path.Combine(path, file);
        return File.ReadAllText(filePath);
    }

    #endregion Helper
}

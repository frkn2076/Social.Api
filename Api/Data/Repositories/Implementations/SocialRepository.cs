using Api.Data.Contracts;
using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.Utils;
using Dapper;
using System.Data;

namespace Api.Data.Repositories.Implementations;

public class SocialRepository : ISocialRepository
{
    private readonly IConnectionService _connectionService;

    public IDbConnection PostgresConnection => _connectionService.GetPostgresConnection();

    public SocialRepository(IConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    public async Task CreateProfileAsync(Profile profile, IDbTransaction transaction = null)
    {
        await PostgresConnection.ExecuteAsync(GetQuery(Queries.CreateProfileQuery), profile, transaction: transaction);
    }

    public async Task<string> GetPasswordAsync(string email, IDbTransaction transaction = null)
    {
        return await PostgresConnection.QueryFirstAsync<string>(GetQuery(Queries.GetPasswordQuery), new { email }, transaction: transaction);
    }

    public async Task<Profile> GetProfileByRefreshTokenAsync(string refreshToken, IDbTransaction transaction = null)
    {
        return await PostgresConnection.QueryFirstAsync<Profile>(GetQuery(Queries.GetProfileByRefreshTokenQuery), new { refreshToken }, transaction: transaction);
    }

    public async Task<Profile> GetProfileByUserNameAsync(string userName, IDbTransaction transaction = null)
    {
        return await PostgresConnection.QueryFirstAsync<Profile>(GetQuery(Queries.GetProfileByUserNameQuery), new { userName }, transaction: transaction);
    }

    public async Task UpdateRefreshTokenAsync(int id, string refreshToken, DateTime expireDate, IDbTransaction transaction = null)
    {
        await PostgresConnection.ExecuteAsync(GetQuery(Queries.UpdateRefreshTokenQuery), new { id, refreshToken, expireDate },
            transaction: transaction);
    }

    #region Helper

    private string GetQuery(string queryFileName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = Path.Combine(currentDirectory, Queries.AdhocFolderPath);
        var file = string.Concat(queryFileName, Queries.SqlFileExtension);
        return FileResourceUtility.LoadResource(path, file);
    }

    #endregion Helper
}

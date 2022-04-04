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

    private string GetQuery(string queryFileName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = Path.Combine(currentDirectory, Queries.AdhocFolderPath);
        var file = string.Concat(queryFileName, Queries.SqlFileExtension);
        return FileResourceUtility.LoadResource(path, file);
    }
}

using Api.Data.Contracts;
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

    public async Task Create()
    {
        await PostgresConnection.ExecuteAsync(GetQuery(Queries.CreateActivityTable));
    }

    private string GetQuery(string queryFileName)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = Path.Combine(currentDirectory, Queries.AdhocFolderPath);
        var file = Path.Combine(queryFileName, Queries.SqlFileExtension);
        return FileResourceUtility.LoadResource(path, file);
    }
}

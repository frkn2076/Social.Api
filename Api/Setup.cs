using Api.Data;
using Api.Data.Contracts;
using Api.Data.Implementations;
using Api.Data.Repositories.Contracts;
using Api.Data.Repositories.Implementations;
using Api.Utils;
using Dapper;
using System.Data;

namespace Api;

public static class Setup
{
    public static void ConfigureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddControllers();

        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();

        serviceCollection.AddTransient<IConnectionService, ConnectionService>();
        serviceCollection.AddTransient<ISocialRepository, SocialRepository>();
    }

    public static async void CreateSchemes(this IDbConnection dbConnection)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var folderPath = Path.Combine(currentDirectory, Queries.SchemeFolderPath);
        var schemeQueryFileNames = Directory.GetFiles(folderPath);

        using var transaction = dbConnection.BeginTransaction();
        foreach (var schemeQueryFileName in schemeQueryFileNames)
        {
            var schemeQuery = FileResourceUtility.LoadResource(folderPath, schemeQueryFileName);
            await dbConnection.ExecuteAsync(schemeQuery, transaction: transaction);
        }
        transaction.Commit();
    }
}

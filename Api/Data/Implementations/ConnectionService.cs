using Api.Data.Contracts;
using Npgsql;
using System.Data;

namespace Api.Data.Implementations;

public class ConnectionService : IConnectionService
{
    private readonly string _connectionString;
    private IDbConnection _dbConnection;

    public ConnectionService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PostgresContext");
    }

    public IDbConnection GetPostgresConnection()
    {
        if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
        {
            return _dbConnection;
        }

        _dbConnection = new NpgsqlConnection(_connectionString);
        _dbConnection.Open();
        return _dbConnection;
    }

    public void CloseConnection()
    {
        if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
        {
            _dbConnection.Close();
        }
    }
}

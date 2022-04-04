using System.Data;

namespace Api.Data.Contracts;

public interface IConnectionService
{
    public IDbConnection GetPostgresConnection();

    public void CloseConnection();
}

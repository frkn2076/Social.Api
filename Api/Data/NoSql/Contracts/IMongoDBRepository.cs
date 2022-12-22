namespace Api.Data.NoSql.Contracts;

public interface IMongoDBRepository
{
    Task InsertMessage(string message);

    Task<string> GetMessages(int roomId);
}

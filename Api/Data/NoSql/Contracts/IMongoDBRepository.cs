namespace Api.Data.NoSql.Contracts;

public interface IMongoDBRepository
{
    Task InsertMessage(string message);

    Task<List<object>?> GetMessages(int roomId);
}

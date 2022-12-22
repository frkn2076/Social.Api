using Api.Data.NoSql.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Api.Data.NoSql.Implementations;

public class MongoDBRepository : IMongoDBRepository
{
    private readonly IMongoCollection<BsonDocument> _chatCollections;
    
    public MongoDBRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoContext");
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("MyMongoDb");
        _chatCollections = database.GetCollection<BsonDocument>("Chats");
    }

    public async Task InsertMessage(string message)
    {
        await _chatCollections.InsertOneAsync(BsonDocument.Parse(message));
    }

    public async Task<string> GetMessages(int roomId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("id", roomId);
        var bsonDocuments = await _chatCollections.Find(filter).ToListAsync();
        var jsonResult = bsonDocuments.ToJson();
        return jsonResult;
    }
}

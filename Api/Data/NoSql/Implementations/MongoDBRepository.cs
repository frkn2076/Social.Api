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

    public async Task<List<object>?> GetMessages(int roomId)
    {
        //var Notfilter = Builders<BsonDocument>.Filter.Empty;
        //_chatCollections.DeleteMany(Notfilter);
        var filter = Builders<BsonDocument>.Filter.Eq("id", roomId);
        var projection = Builders<BsonDocument>.Projection.Include("author").Include("createdAt")
            .Include("id").Include("status").Include("text").Include("type").Exclude("_id");
        var bsonDocuments = _chatCollections.Find(filter).Project(projection).ToList();
        var jsonResult = bsonDocuments.ConvertAll(BsonTypeMapper.MapToDotNetValue);
        return jsonResult;
    }
}

using MongoDB.Driver;

namespace ProductsCRUD.Tests.Infrastructure;

public sealed class MongoTestDatabase
{
    private readonly IMongoClient _client;
    private readonly string _databaseName;

    public MongoTestDatabase(string connectionString, string databaseName)
    {
        _client = new MongoClient(connectionString);
        _databaseName = databaseName;
    }

    public async Task DropAsync()
    {
        var db = _client.GetDatabase(_databaseName);
        
        await db.DropCollectionAsync("Products"); 
    }
}
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductsCRUD.Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ProductsCRUD.Infrastructure.Mongo;

public sealed class MongoDbContext : IMongoDbContext
{
    public IMongoDatabase Database { get; }
    public IMongoCollection<ProductsCRUD.Domain.Entities.Product> Products { get; }

    public MongoDbContext(IOptions<MongoSettings> options)
    {
        var settings = options.Value;

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        var client = new MongoClient(settings.ConnectionString);
        Database = client.GetDatabase(settings.DatabaseName);
        Products = Database.GetCollection<ProductsCRUD.Domain.Entities.Product>("products");
    }
}
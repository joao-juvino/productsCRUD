using MongoDB.Driver;

namespace ProductsCRUD.Infrastructure.Mongo;

public interface IMongoDbContext
{
    IMongoDatabase Database { get; }
    IMongoCollection<Domain.Entities.Product> Products { get; }
}
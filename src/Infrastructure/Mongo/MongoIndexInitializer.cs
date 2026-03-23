using MongoDB.Driver;
using ProductsCRUD.Domain.Entities;

namespace ProductsCRUD.Infrastructure.Mongo;

public sealed class MongoIndexInitializer
{
    private readonly IMongoDbContext _context;

    public MongoIndexInitializer(IMongoDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        var keys = new List<CreateIndexModel<Product>>
        {
            new(Builders<Product>.IndexKeys.Ascending(x => x.Name)),
            new(Builders<Product>.IndexKeys.Ascending(x => x.IsDeleted)),
            new(Builders<Product>.IndexKeys.Descending(x => x.CreatedAt))
        };

        await _context.Products.Indexes.CreateManyAsync(keys, cancellationToken);
    }
}
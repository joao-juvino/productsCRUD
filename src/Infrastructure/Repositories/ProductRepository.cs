using MongoDB.Driver;
using ProductsCRUD.Domain.Entities;
using ProductsCRUD.Domain.Interfaces;
using ProductsCRUD.Infrastructure.Mongo;

namespace ProductsCRUD.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _collection;

    public ProductRepository(IMongoDbContext context)
    {
        _collection = context.Products;
    }

    public Task CreateAsync(Product product, CancellationToken cancellationToken = default) =>
        _collection.InsertOneAsync(product, cancellationToken: cancellationToken);

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public async Task<(IReadOnlyList<Product> Items, long TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        bool? isDeleted,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Product>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchFilter = Builders<Product>.Filter.Or(
                Builders<Product>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(search, "i")),
                Builders<Product>.Filter.Regex(x => x.Description, new MongoDB.Bson.BsonRegularExpression(search, "i")));

            filter &= searchFilter;
        }

        if (isDeleted.HasValue)
        {
            filter &= Builders<Product>.Filter.Eq(x => x.IsDeleted, isDeleted.Value);
        }

        var totalCount = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var items = await _collection.Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default) =>
        _collection.ReplaceOneAsync(x => x.Id == product.Id, product, cancellationToken: cancellationToken);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
}
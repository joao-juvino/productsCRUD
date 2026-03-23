using ProductsCRUD.Domain.Entities;

namespace ProductsCRUD.Domain.Interfaces;

public interface IProductRepository
{
    Task CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Product> Items, long TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        bool? isDeleted,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
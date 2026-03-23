using ProductsCRUD.Application.DTOs.Common;
using ProductsCRUD.Application.DTOs.Products;

namespace ProductsCRUD.Application.Abstractions;

public interface IProductAppService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<ProductResponse>> GetPagedAsync(int page, int pageSize, string? search, bool? isDeleted, CancellationToken cancellationToken = default);
    Task<ProductResponse> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
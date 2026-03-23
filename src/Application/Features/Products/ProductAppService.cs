using ProductsCRUD.Application.Abstractions;
using ProductsCRUD.Application.Common;
using ProductsCRUD.Application.DTOs.Common;
using ProductsCRUD.Application.DTOs.Products;
using ProductsCRUD.Domain.Entities;
using ProductsCRUD.Domain.Exceptions;
using ProductsCRUD.Domain.Interfaces;

namespace ProductsCRUD.Application.Features.Products;

public sealed class ProductAppService : IProductAppService
{
    private readonly IProductRepository _repository;

    public ProductAppService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = new Product(request.Name, request.Description, request.Price, request.Stock);
        await _repository.CreateAsync(product, cancellationToken);
        return Map(product);
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        return product is null ? null : Map(product);
    }

    public async Task<PagedResult<ProductResponse>> GetPagedAsync(int page, int pageSize, string? search, bool? isDeleted, CancellationToken cancellationToken = default)
    {
        page = PaginationHelper.NormalizePage(page);
        pageSize = PaginationHelper.NormalizePageSize(pageSize);

        var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize, search, isDeleted, cancellationToken);

        return new PagedResult<ProductResponse>
        {
            Items = items.Select(Map).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<ProductResponse> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            throw new DomainException("Produto não encontrado.");

        product.Update(request.Name, request.Description, request.Price, request.Stock);
        await _repository.UpdateAsync(product, cancellationToken);

        return Map(product);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            throw new DomainException("Produto não encontrado.");

        product.SoftDelete();
        await _repository.UpdateAsync(product, cancellationToken);
    }

    private static ProductResponse Map(Product product) =>
        new(product.Id, product.Name, product.Description, product.Price, product.Stock, product.IsDeleted, product.CreatedAt, product.UpdatedAt);
}
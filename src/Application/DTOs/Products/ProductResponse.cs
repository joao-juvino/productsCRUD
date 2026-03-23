namespace ProductsCRUD.Application.DTOs.Products;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    bool IsDeleted,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
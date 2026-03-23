namespace ProductsCRUD.Application.DTOs.Products;

public sealed record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int Stock);
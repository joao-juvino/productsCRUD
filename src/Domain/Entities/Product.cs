namespace ProductsCRUD.Domain.Entities;

using ProductsCRUD.Domain.Exceptions;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() { }

    public Product(string name, string description, decimal price, int stock)
    {
        Validate(name, price, stock);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
        Stock = stock;
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void Update(string name, string description, decimal price, int stock)
    {
        if (IsDeleted)
            throw new DomainException("Não é possível atualizar um produto removido.");

        Validate(name, price, stock);

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
        Stock = stock;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        if (IsDeleted) return;
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(string name, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome é obrigatório.");

        if (name.Length > 120)
            throw new DomainException("Nome deve ter no máximo 120 caracteres.");

        if (price <= 0)
            throw new DomainException("Preço deve ser maior que zero.");

        if (stock < 0)
            throw new DomainException("Estoque não pode ser negativo.");
    }
}
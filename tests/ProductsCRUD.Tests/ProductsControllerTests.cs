using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProductsCRUD.Tests.Infrastructure;

namespace ProductsCRUD.Tests;

public sealed class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly MongoTestDatabase _mongoTestDatabase;

    public ProductsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _mongoTestDatabase = new MongoTestDatabase(factory.ConnectionString, factory.DatabaseName);
    }

    public async Task InitializeAsync()
    {
        await _mongoTestDatabase.DropAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Create_ShouldReturnCreatedProduct()
    {
        var request = new
        {
            name = "Mouse Gamer",
            description = "Mouse com RGB",
            price = 150.90m,
            stock = 10
        };

        var response = await _client.PostAsJsonAsync("/api/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        body.Should().NotBeNull();
        body!.Name.Should().Be("Mouse Gamer");
        body.Price.Should().Be(150.90m);
        body.Stock.Should().Be(10);
    }

    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
    {
        var created = await CreateProductAsync();

        var response = await _client.GetAsync($"/api/products/{created.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        body.Should().NotBeNull();
        body!.Id.Should().Be(created.Id);
        body.Name.Should().Be("Teclado Mecânico");
    }

    [Fact]
    public async Task GetPaged_ShouldReturnList_WithPagination()
    {
        await CreateProductAsync("Produto 1");
        await CreateProductAsync("Produto 2");
        await CreateProductAsync("Produto 3");

        var response = await _client.GetAsync("/api/products?page=1&pageSize=2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<PagedResultDto<ProductResponseDto>>();
        body.Should().NotBeNull();
        body!.Items.Should().HaveCount(2);
        body.Page.Should().Be(1);
        body.PageSize.Should().Be(2);
        body.TotalCount.Should().BeGreaterThanOrEqualTo(3);
    }

    [Fact]
    public async Task Update_ShouldChangeProductData()
    {
        var created = await CreateProductAsync();

        var request = new
        {
            name = "Teclado Atualizado",
            description = "Novo texto",
            price = 399.99m,
            stock = 5
        };

        var response = await _client.PutAsJsonAsync($"/api/products/{created.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        body.Should().NotBeNull();
        body!.Name.Should().Be("Teclado Atualizado");
        body.Price.Should().Be(399.99m);
        body.Stock.Should().Be(5);
    }

    [Fact]
    public async Task Delete_ShouldSoftDeleteProduct()
    {
        var created = await CreateProductAsync();

        var response = await _client.DeleteAsync($"/api/products/{created.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/products/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await getResponse.Content.ReadFromJsonAsync<ProductResponseDto>();
        body.Should().NotBeNull();
        body!.IsDeleted.Should().BeTrue();
    }

    private async Task<ProductResponseDto> CreateProductAsync(
        string name = "Teclado Mecânico",
        string description = "Produto de teste",
        decimal price = 250.00m,
        int stock = 20)
    {
        var request = new
        {
            name,
            description,
            price,
            stock
        };

        var response = await _client.PostAsJsonAsync("/api/products", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        body.Should().NotBeNull();

        return body!;
    }

    private sealed record ProductResponseDto(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Stock,
        bool IsDeleted,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    private sealed record PagedResultDto<T>(
        IReadOnlyList<T> Items,
        int Page,
        int PageSize,
        long TotalCount,
        int TotalPages);
}
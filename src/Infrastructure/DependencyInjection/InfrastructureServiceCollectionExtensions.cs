using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductsCRUD.Domain.Interfaces;
using ProductsCRUD.Infrastructure.Mongo;
using ProductsCRUD.Infrastructure.Repositories;
using ProductsCRUD.Infrastructure.Settings;

namespace ProductsCRUD.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection(MongoSettings.SectionName));

        services.AddSingleton<IMongoDbContext, MongoDbContext>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddSingleton<MongoIndexInitializer>();

        return services;
    }
}
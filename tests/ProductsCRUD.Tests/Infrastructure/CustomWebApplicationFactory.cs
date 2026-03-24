using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace ProductsCRUD.Tests.Infrastructure;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public string ConnectionString { get; }
    public string DatabaseName { get; }

    public CustomWebApplicationFactory()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Testing.json", optional: true) 
            .AddUserSecrets<CustomWebApplicationFactory>()          
            .AddEnvironmentVariables()                             
            .Build();

        ConnectionString = configuration["MongoDb:ConnectionString"]
            ?? throw new InvalidOperationException("MongoDb:ConnectionString não encontrado nos Secrets ou JSON.");

        DatabaseName = configuration["MongoDb:DatabaseName"]
            ?? "ProductsCRUD_Tests";
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var values = new Dictionary<string, string?>
            {
                ["MongoDb:ConnectionString"] = ConnectionString,
                ["MongoDb:DatabaseName"] = DatabaseName
            };

            config.AddInMemoryCollection(values);
        });
    }
}
using System.Net;
using ClickHouse.Driver;
using ClickHouse.Driver.ADO;
using ClickHouse.Core.Interfaces.Repositories;
using ClickHouse.Data.Configuration;
using ClickHouse.Data.Migrations;
using ClickHouse.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClickHouse.Data.Extentions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDemoServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ClickHouseSettings>(configuration.GetSection("ClickHouse"));
        services.AddSingleton(provider =>
        {
            var settings = new ClickHouseSettings();
            configuration.GetSection("ClickHouse").Bind(settings);
            return settings;
        });

        services.AddSingleton(provider =>
        {
            var settings = provider.GetRequiredService<ClickHouseSettings>();

            var httpHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All,
                MaxConnectionsPerServer = 16
            };
            var httpClient = new HttpClient(httpHandler);

            var clientSettings = new ClickHouseClientSettings(settings.ConnectionString)
            {
                HttpClient = httpClient
            };

            return new ClickHouseClient(clientSettings);
        });

        services.AddSingleton<ClickHouseMigration>();

        services.AddSingleton<ISensorRepository, SensorRepository>();

        return services;
    }
}

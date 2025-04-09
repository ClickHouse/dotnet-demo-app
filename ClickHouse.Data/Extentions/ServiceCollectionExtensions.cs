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

        services.AddSingleton<ClickHouseMigration>();

        services.AddSingleton<ISensorRepository, SensorRepository>();

        return services;
    }
}

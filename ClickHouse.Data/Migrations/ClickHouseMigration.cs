using ClickHouse.Driver.ADO;
using ClickHouse.Data.Configuration;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ClickHouse.Data.Migrations;

public class ClickHouseMigration
{
    private readonly ClickHouseConnection _connection;
    private readonly ILogger _logger;

    public ClickHouseMigration(ClickHouseSettings settings, ILogger<ClickHouseMigration> logger)
    {
        _connection = new ClickHouseConnection(settings.ConnectionString);
        _logger = logger;
    }

    public async Task MigrateAsync()
    {
        _logger.LogInformation("Starting ClickHouse database migration...");

        await CreateSensorTableAsync();

        _logger.LogInformation("Completed ClickHouse database migration.");
    }

    private async Task CreateSensorTableAsync()
    {
        var sql = @"
            CREATE TABLE IF NOT EXISTS sensors
            (
                sensor_id UInt16,
                sensor_type Enum('BME280', 'BMP180', 'BMP280', 'DHT22', 'DS18B20', 'HPM', 'HTU21D', 'PMS1003', 'PMS3003', 'PMS5003', 'PMS6003', 'PMS7003', 'PPD42NS', 'SDS011'),
                location UInt32,
                lat Float32,
                lon Float32,
                timestamp DateTime,
                P1 Float32,
                P2 Float32,
                P0 Float32,
                durP1 Float32,
                ratioP1 Float32,
                durP2 Float32,
                ratioP2 Float32,
                pressure Float32,
                altitude Float32,
                pressure_sealevel Float32,
                temperature Float32,
                humidity Float32,
                date Date MATERIALIZED toDate(timestamp)
            )
            ENGINE = MergeTree
            ORDER BY (timestamp, sensor_id);
        ";

        await _connection.ExecuteAsync(sql);
        _logger.LogInformation("sensors table created or already exists");
    }
}

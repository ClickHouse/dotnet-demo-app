using ClickHouse.Core.Interfaces.Repositories;
using ClickHouse.Core.Models;
using ClickHouse.Data.Configuration;
using Dapper;

namespace ClickHouse.Data.Repositories;

public class SensorRepository : ClickHouseBaseRepository, ISensorRepository
{
    private readonly string _sensorsTable;

    public SensorRepository(ClickHouseSettings settings) : base(settings)
    {
        _sensorsTable = settings.SensorsTable;
    }

    public async Task<IEnumerable<int>> GetLocationsAsync()
    {
        var sql = @" 
            SELECT location FROM (
                SELECT location, COUNT(*) c
                FROM @SensorsTable
                WHERE
                    pressure > 0
                    AND temperature > 0
                    AND humidity > 0
                GROUP BY location
                ORDER BY c DESC
                LIMIT 100
            )
        ";
        return await _connection.QueryAsync<int>(sql, new { SensorsTable = _sensorsTable });
    }

    public async Task<LocationSample> GetMedianForLocationAndTimeRangeAsync(int locationId)
    {
        var sql = @"
            SELECT
                any(location) AS LocationId,
                any(lat) AS Latitude,
                any(lon) AS Longitude,
                quantile(0.5)(pressure) AS Pressure,
                quantile(0.5)(altitude) AS Altitude,
                quantile(0.5)(pressure_sealevel) AS PressureSeaLevel,
                quantile(0.5)(temperature) AS Temperature,
                quantile(0.5)(humidity) AS Humidity
            FROM @SensorsTable
            WHERE location = @LocationId
            LIMIT 1
        ";
        return await _connection.QueryFirstAsync<LocationSample>(sql, new { SensorsTable = _sensorsTable, LocationId = locationId });
    }

    public async Task<DateTime> GetLatestSensorTimeAsync()
    {
        var sql = $"SELECT max(timestamp) FROM {_sensorsTable}";
        return await _connection.QueryFirstOrDefaultAsync<DateTime>(sql);
    }

    public async Task<ulong> GetTotalSensorsCountAsync()
    {
        var sql = $"SELECT COUNT(DISTINCT sensor_id) FROM {_sensorsTable}";
        return await _connection.QueryFirstOrDefaultAsync<ulong>(sql);
    }

    public async Task<ulong> GetTotalSamplesCountAsync()
    {
        var sql = $"SELECT COUNT(*) FROM {_sensorsTable}";
        return await _connection.QueryFirstOrDefaultAsync<ulong>(sql);
    }

    public async Task<IEnumerable<DateCount>> GetSampleCountsPerDateAsync()
    {
        var sql = $"SELECT toStartOfMonth(timestamp) Date, COUNT(*) Count FROM {_sensorsTable} GROUP BY Date ORDER BY Date DESC LIMIT 12";
        return await _connection.QueryAsync<DateCount>(sql);
    }

    public async Task<IEnumerable<SensorCount>> GetSampleCountsPerSensorAsync()
    {
        var sql = $"SELECT sensor_type::String SensorType, COUNT(*) count FROM {_sensorsTable} GROUP BY sensor_type ORDER BY count DESC LIMIT 12";
        return await _connection.QueryAsync<SensorCount>(sql);
    }
}

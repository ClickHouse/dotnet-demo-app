using ClickHouse.Driver;
using ClickHouse.Core.Interfaces.Repositories;
using ClickHouse.Core.Models;
using ClickHouse.Data.Configuration;
using Dapper;

namespace ClickHouse.Data.Repositories;

public class SensorRepository : ClickHouseBaseRepository, ISensorRepository
{
    private readonly string _sensorsTable;

    public SensorRepository(ClickHouseClient client, ClickHouseSettings settings) : base(client)
    {
        _sensorsTable = settings.SensorsTable;
    }

    public async Task<IEnumerable<int>> GetLocationsAsync()
    {
        var sql = $@"
            SELECT location FROM (
                SELECT location, COUNT(*) c
                FROM {_sensorsTable}
                WHERE
                    pressure > 0
                    AND temperature > 0
                    AND humidity > 0
                GROUP BY location
                ORDER BY c DESC
                LIMIT 100
            )
        ";
        await using var reader = await _client.ExecuteReaderAsync(sql);
        var locations = new List<int>();
        while (reader.Read())
        {
            locations.Add(reader.GetInt32(0));
        }
        return locations;
    }

    public async Task<LocationSample> GetMedianForLocationAndTimeRangeAsync(int locationId)
    {
        var sql = $@"
            SELECT
                any(location) AS LocationId,
                any(lat) AS Latitude,
                any(lon) AS Longitude,
                quantile(0.5)(pressure) AS Pressure,
                quantile(0.5)(altitude) AS Altitude,
                quantile(0.5)(pressure_sealevel) AS PressureSeaLevel,
                quantile(0.5)(temperature) AS Temperature,
                quantile(0.5)(humidity) AS Humidity
            FROM {_sensorsTable}
            WHERE location = @LocationId
            LIMIT 1
        ";

        using var connection = _client.CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryFirstAsync<LocationSample>(sql, new { LocationId = locationId });
    }

    public async Task<DateTime> GetLatestSensorTimeAsync()
    {
        var sql = $"SELECT max(timestamp) FROM {_sensorsTable}";
        var result = await _client.ExecuteScalarAsync(sql);
        return result as DateTime? ?? DateTime.MinValue;
    }

    public async Task<ulong> GetTotalSensorsCountAsync()
    {
        var sql = $"SELECT COUNT(DISTINCT sensor_id) FROM {_sensorsTable}";
        var result = await _client.ExecuteScalarAsync(sql);
        return result as ulong? ?? 0;
    }

    public async Task<ulong> GetTotalSamplesCountAsync()
    {
        var sql = $"SELECT COUNT(*) FROM {_sensorsTable}";
        var result = await _client.ExecuteScalarAsync(sql);
        return result as ulong? ?? 0;
    }

    public async Task<IEnumerable<DateCount>> GetSampleCountsPerDateAsync()
    {
        var sql = $"SELECT toStartOfMonth(timestamp) Date, COUNT(*) Count FROM {_sensorsTable} GROUP BY Date ORDER BY Date DESC LIMIT 12";
        await using var reader = await _client.ExecuteReaderAsync(sql);
        var results = new List<DateCount>();
        while (reader.Read())
        {
            results.Add(new DateCount
            {
                Date = reader.GetDateTime(0),
                Count = reader.GetFieldValue<ulong>(1)
            });
        }
        return results;
    }

    public async Task<IEnumerable<SensorCount>> GetSampleCountsPerSensorAsync()
    {
        var sql = $"SELECT sensor_type::String SensorType, COUNT(*) count FROM {_sensorsTable} GROUP BY sensor_type ORDER BY count DESC LIMIT 12";
        await using var reader = await _client.ExecuteReaderAsync(sql);
        var results = new List<SensorCount>();
        while (reader.Read())
        {
            results.Add(new SensorCount
            {
                SensorType = reader.GetString(0),
                Count = reader.GetFieldValue<ulong>(1)
            });
        }
        return results;
    }
}

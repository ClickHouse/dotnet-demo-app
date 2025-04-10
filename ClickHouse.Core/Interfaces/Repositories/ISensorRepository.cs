using ClickHouse.Core.Models;

namespace ClickHouse.Core.Interfaces.Repositories;

public interface ISensorRepository
{
    Task<IEnumerable<int>> GetLocationsAsync();
    Task<LocationSample> GetMedianForLocationAndTimeRangeAsync(int locationId);
    Task<DateTime> GetLatestSensorTimeAsync();
    Task<ulong> GetTotalSensorsCountAsync();
    Task<ulong> GetTotalSamplesCountAsync();
    Task<IEnumerable<DateCount>> GetSampleCountsPerDateAsync();
    Task<IEnumerable<SensorCount>> GetSampleCountsPerSensorAsync();
}

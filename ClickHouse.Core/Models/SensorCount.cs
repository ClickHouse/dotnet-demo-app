namespace ClickHouse.Core.Models;

public record SensorCount
{
    public string SensorType { get; set; }
    public ulong Count { get; set; }
}
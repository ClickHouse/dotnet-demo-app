namespace ClickHouse.Core.Models;

public record SensorCount
{
    public string SensorType { get; set; }
    public int Count { get; set; }
}
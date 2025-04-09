namespace ClickHouse.Core.Models;

public class DemoListStats
{
    public DateCount[] SamplesPerDate { get; set; }
    public SensorCount[] SamplesPerSensor { get; set; }
}
namespace ClickHouse.Core.Models;

public class DemoStats
{
    public DateTime LatestSensorTime { get; set; }
    public ulong TotalSensors { get; set; }
    public ulong TotalSamples { get; set; }
}

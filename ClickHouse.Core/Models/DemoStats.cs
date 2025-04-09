namespace ClickHouse.Core.Models;

public class DemoStats
{
    public DateTime LatestSensorTime { get; set; }
    public int TotalSensors { get; set; }
    public int TotalSamples { get; set; }
}

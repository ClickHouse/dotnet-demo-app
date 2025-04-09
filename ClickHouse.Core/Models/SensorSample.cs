namespace ClickHouse.Core.Models;

public record SensorSample
{
    public ushort sensor_id { get; set; }
    public string sensor_type { get; set; }
    public uint location { get; set; }
    public float lat { get; set; }
    public float lon { get; set; }
    public DateTime timestamp { get; set; }
    public float P1 { get; set; }
    public float P2 { get; set; }
    public float P0 { get; set; }
    public float durP1 { get; set; }
    public float ratioP1 { get; set; }
    public float durP2 { get; set; }
    public float ratioP2 { get; set; }
    public float pressure { get; set; }
    public float altitude { get; set; }
    public float pressure_sealevel { get; set; }
    public float temperature { get; set; }
    public float humidity { get; set; }
    public DateTime date { get; set; }
}

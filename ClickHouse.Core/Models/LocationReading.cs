namespace ClickHouse.Core.Models;

public record LocationSample
{
    public int LocationId { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public float Pressure { get; set; }
    public float Altitude { get; set; }
    public float PressureSeaLevel { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
};
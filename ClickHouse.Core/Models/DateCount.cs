namespace ClickHouse.Core.Models;

public record DateCount
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
};
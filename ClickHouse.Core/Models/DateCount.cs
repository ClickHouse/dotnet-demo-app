namespace ClickHouse.Core.Models;

public record DateCount
{
    public DateTime Date { get; set; }
    public ulong Count { get; set; }
};
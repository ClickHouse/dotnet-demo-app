namespace ClickHouse.Data.Configuration;

public class ClickHouseSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8123;
    public string Database { get; set; } = "default";
    public string Username { get; set; } = "default";
    public string Password { get; set; } = "";

    public string SensorsTable { get; set; } = Environment.GetEnvironmentVariable("CLICKHOUSE_SENSORS_TABLE") ?? "sensors";

    public string ConnectionString =>
        Environment.GetEnvironmentVariable("CLICKHOUSE_CONNECTION_STRING")
        ?? $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
}

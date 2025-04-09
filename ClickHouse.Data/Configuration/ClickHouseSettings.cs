namespace ClickHouse.Data.Configuration;

public class ClickHouseSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8123;
    public string Database { get; set; } = "default";
    public string Username { get; set; } = "default";
    public string Password { get; set; } = "";

    public string ConnectionString => $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
}

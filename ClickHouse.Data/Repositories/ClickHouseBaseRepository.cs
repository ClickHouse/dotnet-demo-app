using System.Net;
using ClickHouse.Driver.ADO;
using ClickHouse.Data.Configuration;

namespace ClickHouse.Data.Repositories;

public abstract class ClickHouseBaseRepository
{
    protected readonly ClickHouseConnection _connection;

    protected ClickHouseBaseRepository(ClickHouseSettings settings)
    {
        var httpHandler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All,
            MaxConnectionsPerServer = 16
        };
        var httpClient = new HttpClient(httpHandler);

        _connection = new ClickHouseConnection(settings.ConnectionString, httpClient);
    }
}

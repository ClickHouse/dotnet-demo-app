using ClickHouse.Driver;

namespace ClickHouse.Data.Repositories;

public abstract class ClickHouseBaseRepository
{
    protected readonly ClickHouseClient _client;

    protected ClickHouseBaseRepository(ClickHouseClient client)
    {
        _client = client;
    }
}

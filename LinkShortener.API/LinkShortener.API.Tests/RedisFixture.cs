using StackExchange.Redis;

public class RedisFixture : IAsyncLifetime
{
    public IConnectionMultiplexer? Connection { get; private set; }

    public async Task InitializeAsync()
    {
        Connection = await ConnectionMultiplexer.ConnectAsync("localhost:6380");
    }

    public async Task DisposeAsync()
    {
        if (Connection != null)
        {
            await Connection.CloseAsync();
            Connection.Dispose();
        }
    }
}
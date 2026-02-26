using StackExchange.Redis;

namespace LinkShortener.API.Tests;

[Collection("Redis Collection")] 
public class RedisTests : IAsyncLifetime
{
    private readonly RedisFixture _redisFixture;
    private readonly IDatabase _database;
    public RedisTests(RedisFixture redisFixture)
    {
        _redisFixture = redisFixture;
        _database = _redisFixture.Connection.GetDatabase();
    }
    public async Task InitializeAsync()
    {
        await _database.ExecuteAsync("FLUSHALL");
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task Test_SetAndGetValue()
    {
        const string key = "test-key";
        const string value = "test-value";
        
        await _database.StringSetAsync(key, value);
        var retrivedValue = await _database.StringGetAsync(key);
        Assert.Equal(value, retrivedValue);
    }

    [Fact]
    public async Task Test_KeyDoesNotExistAfterFlush()
    {
        bool exists = await _database.KeyExistsAsync("test-key");
        Assert.False(exists);
    }
}
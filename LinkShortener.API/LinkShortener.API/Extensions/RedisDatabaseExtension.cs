using LinkShortener.API.Interface;
using LinkShortener.API.Services;
using StackExchange.Redis;

namespace LinkShortener.API.Extensions;

public static class RedisDatabaseExtension
{
    public static IServiceCollection AddRedisDatabase(this IServiceCollection services, IConfiguration conf)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = conf.GetConnectionString("RedisConnection");
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddScoped<IRedisCacheService, RedisCacheService>();
        return services;
    }
}
using System.Text.Json;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orders.Api.Events;
using StackExchange.Redis;

namespace Orders.Api.Repositories;

public class RedisEventStoreRepository : IEventStoreRepository
{
    private readonly IOptions<RedisConfig> _redisConfigOptions;
    private readonly ILogger<RedisEventStoreRepository> _logger;
    private readonly IDatabase _redisDatabase;


    public RedisEventStoreRepository(IOptions<RedisConfig> redisConfigOptions,
        ILogger<RedisEventStoreRepository> logger,
        IDatabase redisDatabase)
    {
        _redisConfigOptions = redisConfigOptions;
        _logger = logger;
        _redisDatabase = redisDatabase;
    }

    public async Task PublishAsync(IEvent message)
    {
        Guard.Against.Null(message);
        var @event = new[] { new NameValueEntry(message.GetType().FullName,  JsonConvert.SerializeObject(message)) };

        await _redisDatabase.StreamAddAsync(_redisConfigOptions.Value.StreamName, @event);
    }
}
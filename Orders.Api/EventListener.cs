using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orders.Api.Events;
using Orders.Api.Events.EventHandlers;
using StackExchange.Redis;

namespace Orders.Api;

public class EventListener : IEventListener
{
    private readonly IDatabase _redisDatabase;
    private readonly IOptions<RedisConfig> _redisConfig;
    private readonly ILogger<EventListener> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EventListener(
        IDatabase redisDatabase,
        IOptions<RedisConfig> redisConfig,
        ILogger<EventListener> logger,
        IServiceProvider serviceProvider)
    {
        _redisDatabase = redisDatabase;
        _redisConfig = redisConfig;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task Listen(CancellationToken token)
    {
        try
        {
            // Read events from the stream
            var lastId = "-";
            while (!token.IsCancellationRequested)
            {
                var result = await _redisDatabase.StreamRangeAsync(_redisConfig.Value.StreamName, lastId, "+");
                if (!result.Any() || lastId == result.Last().Id) continue;
            
                lastId = result.Last().Id;
                
                foreach (var entry in result)
                foreach (var field in entry.Values)
                {
                    var type = Type.GetType(field.Name!);
                    var body = (IEvent)JsonConvert.DeserializeObject(field.Value!, type!)!;

                    // initialize and call our event handler 
                    var messageHandlerType = typeof(IEventHandler<>).MakeGenericType(type!);
                    using var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService(messageHandlerType);
                    
                    handler.GetType().GetMethod("HandleAsync", new[] { type! })?.Invoke(handler, new[] { body });
                }
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e, "an error occured processing events.");
        }
    }
}

public interface IEventListener
{
    Task Listen(CancellationToken token);
}
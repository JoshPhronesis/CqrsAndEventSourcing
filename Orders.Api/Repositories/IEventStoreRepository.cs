using Orders.Api.Events;

namespace Orders.Api.Repositories;

public interface IEventStoreRepository
{
    Task PublishAsync(IEvent message);
}
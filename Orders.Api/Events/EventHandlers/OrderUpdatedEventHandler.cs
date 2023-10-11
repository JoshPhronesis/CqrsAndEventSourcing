using Ardalis.GuardClauses;
using Orders.Api.Repositories;

namespace Orders.Api.Events.EventHandlers;

public class OrderUpdatedEventHandler : IEventHandler<OrderUpdatedEvent>
{
    private readonly IWriteOrdersRepository _repository;

    public OrderUpdatedEventHandler(IWriteOrdersRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(OrderUpdatedEvent @event)
    {
        Guard.Against.Null(@event);

        await _repository.UpdateOrderAsync(@event.Order);
    }
}
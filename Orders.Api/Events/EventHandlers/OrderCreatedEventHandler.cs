using Ardalis.GuardClauses;
using Orders.Api.Repositories;

namespace Orders.Api.Events.EventHandlers;

public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IWriteOrdersRepository _repository;
    private readonly IReadOrdersRepository _readOrdersRepository;

    public OrderCreatedEventHandler(IWriteOrdersRepository repository, IReadOrdersRepository readOrdersRepository)
    {
        _repository = repository;
        _readOrdersRepository = readOrdersRepository;
    }
    public async Task HandleAsync(OrderCreatedEvent @event)
    {
        Guard.Against.Null(@event);

        var order = await _readOrdersRepository.GetOrderByIdAsync(@event.Order.Id);
        if (order is not {})
        {
            await _repository.CreateOrderAsync(@event.Order);
        }
        else
        {
            await _repository.UpdateOrderAsync(@event.Order);
        }
    }
}
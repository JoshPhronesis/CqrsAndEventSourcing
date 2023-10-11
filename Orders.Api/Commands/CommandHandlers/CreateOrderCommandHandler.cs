using Ardalis.GuardClauses;
using Orders.Api.Events;
using Orders.Api.Repositories;

namespace Orders.Api.Commands.CommandHandlers;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public CreateOrderCommandHandler(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }
    public async Task HandleAsync(CreateOrderCommand command)
    {
        Guard.Against.Null(command);

        // persisting data to our event store
        await _eventStoreRepository.PublishAsync(new OrderCreatedEvent(command.Order));
    }
}
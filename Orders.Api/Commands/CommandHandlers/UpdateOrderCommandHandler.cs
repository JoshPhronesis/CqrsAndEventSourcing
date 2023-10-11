using Ardalis.GuardClauses;
using Orders.Api.Events;
using Orders.Api.Repositories;

namespace Orders.Api.Commands.CommandHandlers;

public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand>
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public UpdateOrderCommandHandler(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }
    public async Task HandleAsync(UpdateOrderCommand command)
    {
        Guard.Against.Null(command?.Order);

        await _eventStoreRepository.PublishAsync(new OrderUpdatedEvent(command.Order));
    }
}
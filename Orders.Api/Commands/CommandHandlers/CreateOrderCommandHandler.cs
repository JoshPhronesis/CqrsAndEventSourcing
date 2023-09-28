using Ardalis.GuardClauses;
using Orders.Api.Repositories;

namespace Orders.Api.Commands.CommandHandlers;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
{
    private readonly IWriteOrdersRepository _writeOrdersRepository;

    public CreateOrderCommandHandler(IWriteOrdersRepository writeOrdersRepository)
    {
        _writeOrdersRepository = writeOrdersRepository;
    }
    public async Task HandleAsync(CreateOrderCommand command)
    {
        Guard.Against.Null(command);

        await _writeOrdersRepository.CreateOrderAsync(command.Order);
    }
}
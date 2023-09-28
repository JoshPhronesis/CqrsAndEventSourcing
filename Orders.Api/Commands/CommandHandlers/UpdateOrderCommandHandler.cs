using Ardalis.GuardClauses;
using Orders.Api.Repositories;

namespace Orders.Api.Commands.CommandHandlers;

public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand>
{
    private readonly IWriteOrdersRepository _writeOrdersRepository;

    public UpdateOrderCommandHandler(IWriteOrdersRepository writeOrdersRepository)
    {
        _writeOrdersRepository = writeOrdersRepository;
    }
    public async Task HandleAsync(UpdateOrderCommand command)
    {
        Guard.Against.Null(command?.Order);

        await  _writeOrdersRepository.UpdateOrderAsync(command.Order);
    }
}
using Orders.Api.Entities;

namespace Orders.Api.Commands;

public class UpdateOrderCommand : ICommand
{
    public Order Order { get; }

    public UpdateOrderCommand(Order order)
    {
        Order = order;
    }
}
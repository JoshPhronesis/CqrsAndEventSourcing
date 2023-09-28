using Orders.Api.Entities;

namespace Orders.Api.Commands;

public class CreateOrderCommand : ICommand
{
    public Order Order { get; }

    public CreateOrderCommand(Order order)
    {
        Order = order;
    }
}
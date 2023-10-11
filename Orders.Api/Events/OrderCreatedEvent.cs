using Orders.Api.Entities;

namespace Orders.Api.Events;

public class OrderCreatedEvent : IEvent
{
    public Order Order { get; }

    public OrderCreatedEvent(Order order)
    {
        Order = order;
    }
}
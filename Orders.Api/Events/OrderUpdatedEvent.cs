using Orders.Api.Entities;

namespace Orders.Api.Events;

public class OrderUpdatedEvent : IEvent
{
    public Order Order { get; }

    public OrderUpdatedEvent(Order order)
    {
        Order = order;
    }
}
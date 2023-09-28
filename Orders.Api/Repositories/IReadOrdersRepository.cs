using Orders.Api.Entities;

namespace Orders.Api.Repositories;

public interface IReadOrdersRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderByIdAsync(Guid orderId);
}
using Orders.Api.Data;
using Orders.Api.Entities;

namespace Orders.Api.Repositories;

public class ReadOrdersRepository : IReadOrdersRepository
{
    private readonly AppDbContext _dbContext;

    public ReadOrdersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        var result = await _dbContext.Orders.FindAsync(orderId);
        
        return result;
    }

    public Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return Task.FromResult(_dbContext.Orders.AsEnumerable());
    }
}
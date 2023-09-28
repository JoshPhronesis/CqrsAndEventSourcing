using Ardalis.GuardClauses;
using Orders.Api.Data;
using Orders.Api.Entities;

namespace Orders.Api.Repositories;

public class WriteOrdersRepository : IWriteOrdersRepository
{
    private readonly AppDbContext _dbContext;

    public WriteOrdersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Order> CreateOrderAsync(Order entity)
    {
        Guard.Against.Null(entity);
        
        await _dbContext.Orders.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<Order> UpdateOrderAsync(Order entity)
    {
        Guard.Against.Null(entity);
        
        _dbContext.Orders.Update(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity;
    }

   
    public async Task<bool> DeleteOrderByIdAsync(Order entity)
    {
        Guard.Against.Null(entity);

        _dbContext.Orders.Remove(entity);
        await _dbContext.SaveChangesAsync();

        return true;
    }

}
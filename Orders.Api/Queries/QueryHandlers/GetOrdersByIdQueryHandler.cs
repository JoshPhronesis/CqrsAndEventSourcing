using Ardalis.GuardClauses;
using Orders.Api.Entities;
using Orders.Api.Repositories;

namespace Orders.Api.Queries.QueryHandlers;

public class GetOrdersByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, Order?>
{
    private readonly IReadOrdersRepository _readOrdersRepository;

    public GetOrdersByIdQueryHandler(IReadOrdersRepository readOrdersRepository)
    {
        _readOrdersRepository = readOrdersRepository;
    }
    public async Task<Order?> HandleAsync(GetOrderByIdQuery input)
    {
        Guard.Against.Default(input?.OrderId);
        return  await _readOrdersRepository.GetOrderByIdAsync(input.OrderId);
    }
}
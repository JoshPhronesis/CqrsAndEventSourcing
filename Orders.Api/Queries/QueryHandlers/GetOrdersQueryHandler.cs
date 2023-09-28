using Ardalis.GuardClauses;
using Orders.Api.Entities;
using Orders.Api.Repositories;

namespace Orders.Api.Queries.QueryHandlers;

public class GetOrdersQueryHandler : IQueryHandler<GetOrderQuery, IEnumerable<Order>>
{
    private readonly IReadOrdersRepository _readOrdersRepository;

    public GetOrdersQueryHandler(IReadOrdersRepository readOrdersRepository)
    {
        _readOrdersRepository = readOrdersRepository;
    }
    public async Task<IEnumerable<Order>> HandleAsync(GetOrderQuery input)
    {
        Guard.Against.Null(input);
        return await _readOrdersRepository.GetOrdersAsync();
    }
}
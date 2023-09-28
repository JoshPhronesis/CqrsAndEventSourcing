using Orders.Api.Entities;

namespace Orders.Api.Queries.QueryHandlers;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery
{
    Task<TResponse> HandleAsync(TQuery input);
}
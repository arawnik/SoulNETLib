using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Abstractions.CQRS;

/// <summary>
/// Defines a handler for a query that retrieves data without modifying application state.
/// Queries are intended to return lightweight data transfer objects or projections, not domain entities.
/// </summary>
/// <typeparam name="TQuery">The type of query to handle.</typeparam>
/// <typeparam name="TResponse">The type of result returned by the query.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// Executes the query asynchronously and returns the result.
    /// </summary>
    /// <param name="query">The query to handle.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A <see cref="Result{TResponse}"/> containing the query result.</returns>
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}

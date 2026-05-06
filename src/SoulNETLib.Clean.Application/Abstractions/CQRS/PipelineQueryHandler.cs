using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Abstractions.CQRS;

/// <summary>
/// Defines a pipeline behavior that wraps a query handler returning <see cref="Result{TResponse}"/>.
/// Behaviors execute in registration order, forming a chain where each behavior
/// can inspect the query, short-circuit execution, or perform cross-cutting logic
/// (caching, logging, authorization) before calling the next step.
/// </summary>
/// <typeparam name="TQuery">The type of query being processed.</typeparam>
/// <typeparam name="TResponse">The type of the response value on success.</typeparam>
/// <remarks>
/// <para>
/// Query pipeline behaviors are typically used for read-side concerns such as
/// caching, logging, or authorization checks — not for validation or trimming,
/// which are command-side concerns.
/// </para>
/// <para>
/// Behaviors are resolved from DI as <c>IEnumerable&lt;IQueryPipelineBehavior&lt;TQuery, TResponse&gt;&gt;</c>
/// and executed in registration order.
/// </para>
/// </remarks>
public interface IQueryPipelineBehavior<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// Executes this behavior in the pipeline chain.
    /// </summary>
    /// <param name="query">The query being processed.</param>
    /// <param name="next">The next step in the pipeline (either another behavior or the handler).</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Result{TResponse}"/> from either this behavior (short-circuit) or the next step.
    /// </returns>
    Task<Result<TResponse>> Handle(
        TQuery query,
        PipelineStep<TResponse> next,
        CancellationToken cancellationToken
    );
}

/// <summary>
/// A query handler wrapper that executes registered <see cref="IQueryPipelineBehavior{TQuery, TResponse}"/>
/// behaviors in order before delegating to the inner handler.
/// </summary>
/// <typeparam name="TQuery">The type of query being processed.</typeparam>
/// <typeparam name="TResponse">The type of the response value on success.</typeparam>
/// <remarks>
/// <para>
/// This class acts as the DI-resolved <see cref="IQueryHandler{TQuery, TResponse}"/>. It builds a
/// delegate chain from all registered behaviors and terminates with the actual handler.
/// Behaviors execute in registration order (first registered = outermost).
/// </para>
/// <para>
/// When no behaviors are registered, the inner handler is called directly with no overhead.
/// </para>
/// </remarks>
public sealed class PipelineQueryHandler<TQuery, TResponse>(
    IQueryHandler<TQuery, TResponse> inner,
    IEnumerable<IQueryPipelineBehavior<TQuery, TResponse>> behaviors
) : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    private readonly IQueryHandler<TQuery, TResponse> _inner = inner;
    private readonly IEnumerable<IQueryPipelineBehavior<TQuery, TResponse>> _behaviors = behaviors;

    /// <inheritdoc/>
    public Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
    {
        PipelineStep<TResponse> handler = () => _inner.Handle(query, cancellationToken);

        var pipeline = _behaviors
            .Reverse()
            .Aggregate(
                handler,
                (next, behavior) => () => behavior.Handle(query, next, cancellationToken)
            );

        return pipeline();
    }
}

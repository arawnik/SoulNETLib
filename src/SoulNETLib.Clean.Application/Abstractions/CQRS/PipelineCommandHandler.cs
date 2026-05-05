using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Abstractions.CQRS;

/// <summary>
/// A command handler wrapper that executes registered <see cref="IPipelineBehavior{TCommand}"/>
/// behaviors in order before delegating to the inner handler.
/// </summary>
/// <typeparam name="TCommand">The type of command being processed.</typeparam>
/// <remarks>
/// <para>
/// This class acts as the DI-resolved <see cref="ICommandHandler{TCommand}"/>. It builds a
/// delegate chain from all registered behaviors and terminates with the actual handler.
/// Behaviors execute in registration order (first registered = outermost).
/// </para>
/// <para>
/// When no behaviors are registered, the inner handler is called directly with no overhead.
/// </para>
/// </remarks>
public sealed class PipelineCommandHandler<TCommand>(
    ICommandHandler<TCommand> inner,
    IEnumerable<IPipelineBehavior<TCommand>> behaviors
) : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _inner = inner;
    private readonly IEnumerable<IPipelineBehavior<TCommand>> _behaviors = behaviors;

    /// <inheritdoc/>
    public Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
    {
        PipelineStep handler = () => _inner.Handle(command, cancellationToken);

        var pipeline = _behaviors
            .Reverse()
            .Aggregate(
                handler,
                (next, behavior) => () => behavior.Handle(command, next, cancellationToken)
            );

        return pipeline();
    }
}

/// <summary>
/// A command handler wrapper that executes registered <see cref="IPipelineBehavior{TCommand, TResponse}"/>
/// behaviors in order before delegating to the inner handler.
/// </summary>
/// <typeparam name="TCommand">The type of command being processed.</typeparam>
/// <typeparam name="TResponse">The type of the response value on success.</typeparam>
/// <remarks>
/// <para>
/// This class acts as the DI-resolved <see cref="ICommandHandler{TCommand, TResponse}"/>. It builds a
/// delegate chain from all registered behaviors and terminates with the actual handler.
/// Behaviors execute in registration order (first registered = outermost).
/// </para>
/// <para>
/// When no behaviors are registered, the inner handler is called directly with no overhead.
/// </para>
/// </remarks>
public sealed class PipelineCommandHandler<TCommand, TResponse>(
    ICommandHandler<TCommand, TResponse> inner,
    IEnumerable<IPipelineBehavior<TCommand, TResponse>> behaviors
) : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _inner = inner;
    private readonly IEnumerable<IPipelineBehavior<TCommand, TResponse>> _behaviors = behaviors;

    /// <inheritdoc/>
    public Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
    {
        PipelineStep<TResponse> handler = () => _inner.Handle(command, cancellationToken);

        var pipeline = _behaviors
            .Reverse()
            .Aggregate(
                handler,
                (next, behavior) => () => behavior.Handle(command, next, cancellationToken)
            );

        return pipeline();
    }
}

using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Abstractions.CQRS;

/// <summary>
/// Delegate representing the next step in a pipeline for operations returning <see cref="Result"/>.
/// </summary>
/// <returns>A task producing the operation result.</returns>
public delegate Task<Result> PipelineStep();

/// <summary>
/// Delegate representing the next step in a pipeline for operations returning <see cref="Result{TResponse}"/>.
/// </summary>
/// <typeparam name="TResponse">The type of the response value.</typeparam>
/// <returns>A task producing the operation result.</returns>
public delegate Task<Result<TResponse>> PipelineStep<TResponse>();

/// <summary>
/// Defines a pipeline behavior that wraps a command handler returning <see cref="Result"/>.
/// Behaviors execute in registration order, forming a chain where each behavior
/// can inspect or modify the command, short-circuit execution, or perform
/// cross-cutting logic (validation, logging, trimming) before calling the next step.
/// </summary>
public interface IPipelineBehavior<in TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Executes this behavior in the pipeline chain.
    /// </summary>
    /// <param name="command">The command being processed.</param>
    /// <param name="next">The next step in the pipeline (either another behavior or the handler).</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Result"/> from either this behavior (short-circuit) or the next step.
    /// </returns>
    Task<Result> Handle(TCommand command, PipelineStep next, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a pipeline behavior that wraps a command handler returning <see cref="Result{TResponse}"/>.
/// Behaviors execute in registration order, forming a chain where each behavior
/// can inspect or modify the command, short-circuit execution, or perform
/// cross-cutting logic (validation, logging, trimming) before calling the next step.
/// </summary>
/// <typeparam name="TCommand">The type of command being processed.</typeparam>
/// <typeparam name="TResponse">The type of the response value on success.</typeparam>
public interface IPipelineBehavior<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// Executes this behavior in the pipeline chain.
    /// </summary>
    /// <param name="command">The command being processed.</param>
    /// <param name="next">The next step in the pipeline (either another behavior or the handler).</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Result{TResponse}"/> from either this behavior (short-circuit) or the next step.
    /// </returns>
    Task<Result<TResponse>> Handle(
        TCommand command,
        PipelineStep<TResponse> next,
        CancellationToken cancellationToken
    );
}

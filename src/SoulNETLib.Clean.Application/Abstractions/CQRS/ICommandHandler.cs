using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Abstractions.CQRS;

/// <summary>
/// Defines a handler for a command that performs an operation without returning a result object.
/// Used for write-only operations that modify application state.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle.</typeparam>
public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Executes the command asynchronously.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure of the operation.</returns>
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for a command that performs an operation and returns a result of type <typeparamref name="TResponse"/>.
/// This is used when the command execution should return data such as IDs, status info, or summaries — but never full domain entities.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle.</typeparam>
/// <typeparam name="TResponse">The type of result returned by the command.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// Executes the command asynchronously and returns a result.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A <see cref="Result{TResponse}"/> containing the result of the operation.</returns>
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}

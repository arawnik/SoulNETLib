using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Abstractions.Validation;

/// <summary>
/// Defines a validator for a specific command type.
/// Validators are resolved from DI as <c>IEnumerable&lt;ICommandValidator&lt;TCommand&gt;&gt;</c>
/// and executed by <see cref="Behaviors.ValidationBehavior{TCommand}"/> before the handler runs.
/// </summary>
/// <typeparam name="TCommand">The type of command to validate.</typeparam>
/// <remarks>
/// <para>
/// All registered validators for a command are executed — validation does not short-circuit
/// on the first failure. Errors from all validators are aggregated into a single result.
/// </para>
/// <para>
/// The async signature supports validators that require I/O (database uniqueness checks,
/// external service calls). For simple in-memory validation, return
/// <c>Task.FromResult(errors)</c>.
/// </para>
/// </remarks>
public interface ICommandValidator<in TCommand>
    where TCommand : IBaseCommand
{
    /// <summary>
    /// Validates the given command asynchronously.
    /// </summary>
    /// <param name="command">The command to validate.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// An array of <see cref="Error"/> instances describing validation failures.
    /// Return an empty array if the command is valid.
    /// </returns>
    Task<Error[]> ValidateAsync(TCommand command, CancellationToken cancellationToken);
}

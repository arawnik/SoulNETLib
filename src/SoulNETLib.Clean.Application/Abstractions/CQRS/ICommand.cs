namespace SoulNETLib.Clean.Application.Abstractions.CQRS;

/// <summary>
/// Represents a command that performs an operation without returning any full data objects.
/// Used for actions / operations that modify data.
/// </summary>
public interface ICommand : IBaseCommand;

/// <summary>
/// Represents a command that performs an operation and returns a result of type <typeparamref name="TResponse"/>.
/// Commonly used when the command handler needs to return information related to data operation.
/// This should never be full on data objects, but rather a result of the operation, such as success status, IDs, or other relevant information.
/// </summary>
/// <typeparam name="TResponse">The type of result returned by the command.</typeparam>
public interface ICommand<TResponse> : IBaseCommand;

/// <summary>
/// Base marker interface for all commands in the CQRS system.
/// Used to enable generic processing and behaviors.
/// </summary>
public interface IBaseCommand;

namespace SoulNETLib.Clean.Domain;

/// <summary>
/// Dispatches domain events to their registered handlers.
/// Typically called after a successful <c>SaveChangesAsync</c> to ensure
/// side effects only run when the triggering state change is persisted.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches all provided domain events to their respective handlers.
    /// </summary>
    /// <param name="domainEvents">The events to dispatch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default
    );
}

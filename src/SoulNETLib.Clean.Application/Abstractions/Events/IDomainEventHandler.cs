using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Abstractions.Events;

/// <summary>
/// Defines a handler for a specific domain event type.
/// Implementations contain the side-effect logic that should run after the event is persisted.
/// </summary>
/// <typeparam name="TEvent">The domain event type to handle.</typeparam>
public interface IDomainEventHandler<in TEvent>
    where TEvent : IDomainEvent
{
    /// <summary>
    /// Handles the domain event.
    /// </summary>
    /// <param name="domainEvent">The event instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}

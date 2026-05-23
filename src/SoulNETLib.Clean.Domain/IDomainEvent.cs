namespace SoulNETLib.Clean.Domain;

/// <summary>
/// Marker interface for domain events.
/// Domain events represent something meaningful that happened in the domain.
/// They are raised by entities and dispatched after persistence succeeds.
/// </summary>
public interface IDomainEvent;

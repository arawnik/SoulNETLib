namespace SoulNETLib.Clean.Application.Abstractions.CQRS;

/// <summary>
/// Represents a query that retrieves data and returns a result of type <typeparamref name="TResponse"/>.
/// Queries should never modify state and are intended solely for reading or projecting data.
/// </summary>
/// <typeparam name="TResponse">The type of result returned by the query, typically a DTO or a lightweight view model.</typeparam>
public interface IQuery<TResponse>;

namespace SoulNETLib.Clean.Domain;

/// <summary>
/// Represents an error in an operation.
/// </summary>
public sealed record Error(string Code, string Message)
{
    #region ErrorType methods

    /// <summary>
    /// Creates a "Not Found" error.
    /// </summary>
    public static Error NotFound(string message = "The requested resource was not found.") =>
        new(ErrorCodes.NotFound, message);

    /// <summary>
    /// Creates a "Not Found" error.
    /// </summary>
    public static Error NotFound<TKey>(string type, TKey key) =>
        new(ErrorCodes.NotFound, $"{type} with ID {key} not found.");

    /// <summary>
    /// Creates a business rule violation error. These errors are typically issues that would lead to invalid state.
    /// </summary>
    public static Error BusinessRule(string message) => new(ErrorCodes.BusinessRule, message);

    /// <summary>
    /// Creates an invalid data error. These errors are typically issue that rose from database.
    /// </summary>
    public static Error InvalidData(string message) => new(ErrorCodes.InvalidData, message);

    #endregion

    #region Helper methods

    /// <summary>
    /// Creates an error from an exception.
    /// </summary>
    public static Error FromException(Exception exception, string code = ErrorCodes.General) =>
        new(code, exception?.Message ?? string.Empty);

    #endregion

    /// <summary>
    /// Converts the error into a failed <see cref="Result"/>.
    /// </summary>
    public Result ToResult() => Result.Failure(this);

    /// <summary>
    /// Converts the error into a failed <see cref="Result{T}"/>.
    /// </summary>
    public Result<T> ToResult<T>() => Result<T>.Failure(this);
}

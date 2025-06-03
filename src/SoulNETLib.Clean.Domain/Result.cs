namespace SoulNETLib.Clean.Domain;

/// <summary>
/// Represents the outcome of an operation that does not return a value.
/// </summary>
public class Result : IResult
{
    /// <inheritdoc/>
    public bool IsSuccess { get; }

    /// <inheritdoc/>
    public bool IsFailure => !IsSuccess;

    /// <inheritdoc/>
    public Error? Error { get; }

    /// <summary>
    /// Initializes a result with success status and an optional error.
    /// </summary>
    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result without a value.
    /// </summary>
    public static Result Success() => new(true, null);

    /// <summary>
    /// Creates a successful result with a value, inferring the generic type automatically.
    /// </summary>
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);

    /// <summary>
    /// Creates a failure result with an error.
    /// </summary>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Creates a failure result from an exception.
    /// </summary>
    public static Result Failure(Exception exception) => new(false, Error.FromException(exception));

    /// <summary>
    /// Implicitly converts an error into a failure result.
    /// </summary>
    public static implicit operator Result(Error error) => Failure(error);

    /// <summary>
    /// Implicitly converts an exception into a failure result.
    /// </summary>
    public static implicit operator Result(Exception exception) => Failure(exception);
}

namespace SoulNETLib.Clean.Domain;

/// <summary>
/// Represents the outcome of an operation, which can be either success or failure.
/// </summary>
/// <typeparam name="T">The type of the value returned on success.</typeparam>
public class Result<T> : IResult
{
    /// <inheritdoc/>
    public bool IsSuccess { get; }

    /// <inheritdoc/>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// The value returned when the operation is successful.
    /// </summary>
    public T? Value { get; }

    /// <inheritdoc/>
    public Error? Error { get; }

    /// <summary>
    /// Initializes a successful result with the given value.
    /// </summary>
    protected Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
    }

    /// <summary>
    /// Initializes a failed result with the given error.
    /// </summary>
    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
        Value = default;
    }

    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    public static Result<T> Success(T value) => new(value);

    /// <summary>
    /// Creates a failure result with an error.
    /// </summary>
    public static Result<T> Failure(Error error) => new(error);

    /// <summary>
    /// Creates a failure result from an exception.
    /// </summary>
    public static Result<T> Failure(Exception exception) => new(Error.FromException(exception));

    /// <summary>
    /// Implicitly converts an error into a failure result.
    /// </summary>
    public static implicit operator Result<T>(Error error) => Failure(error);

    /// <summary>
    /// Implicitly converts an exception into a failure result.
    /// </summary>
    public static implicit operator Result<T>(Exception exception) => Failure(exception);
}

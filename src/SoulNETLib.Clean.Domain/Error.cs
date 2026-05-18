using System.Globalization;

namespace SoulNETLib.Clean.Domain;

/// <summary>
/// Represents an error in an operation, optionally associated with a specific field or property.
/// </summary>
/// <param name="Code">A machine-readable error classification code (see <see cref="ErrorCodes"/>).</param>
/// <param name="Message">A human-readable description of the error.</param>
/// <param name="Field">
/// The name of the field or property that caused the error, or <c>null</c> for general errors.
/// When present, follows the same naming convention as
/// <see href="https://datatracker.ietf.org/doc/html/rfc9457">RFC 9457</see> ProblemDetails
/// validation errors (e.g. <c>"EndDate"</c>, <c>"Translations[0].Title"</c>).
/// </param>
public sealed record Error(string Code, string Message, string? Field = null)
{
    #region ErrorType methods

    /// <summary>
    /// Creates a "Not Found" error.
    /// </summary>
    public static Error NotFound(string message = "The requested resource was not found.") =>
        new(ErrorCodes.NotFound, message);

    /// <summary>
    /// Creates a "Not Found" error with a formatted message template.
    /// </summary>
    /// <param name="template">A composite format string (e.g. <c>"{0} with ID {1} not found."</c>).</param>
    /// <param name="args">Format arguments to substitute into the template.</param>
    public static Error NotFound(string template, params object[] args) =>
        new(ErrorCodes.NotFound, string.Format(CultureInfo.InvariantCulture, template, args));

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
    /// Creates a business rule violation error with a formatted message template.
    /// </summary>
    /// <param name="template">A composite format string (e.g. <c>"{0} has already been closed."</c>).</param>
    /// <param name="args">Format arguments to substitute into the template.</param>
    public static Error BusinessRule(string template, params object[] args) =>
        new(ErrorCodes.BusinessRule, string.Format(CultureInfo.InvariantCulture, template, args));

    /// <summary>
    /// Creates an invalid data error. These errors are typically issue that rose from database.
    /// </summary>
    public static Error InvalidData(string message) => new(ErrorCodes.InvalidData, message);

    /// <summary>
    /// Creates a validation error associated with a specific field or property.
    /// </summary>
    /// <param name="field">The field or property name that failed validation (e.g. <c>"EndDate"</c>).</param>
    /// <param name="message">A human-readable description of the validation failure.</param>
    public static Error Validation(string field, string message) =>
        new(ErrorCodes.Validation, message, field);

    /// <summary>
    /// Creates a validation error associated with a specific field, formatting the message from a template.
    /// </summary>
    /// <param name="field">The field or property name that failed validation (e.g. <c>"EndDate"</c>).</param>
    /// <param name="template">A composite format string (e.g. <c>"'{0}' must not be empty."</c>).</param>
    /// <param name="args">Format arguments to substitute into the template.</param>
    public static Error Validation(string field, string template, params object[] args) =>
        new(
            ErrorCodes.Validation,
            string.Format(CultureInfo.InvariantCulture, template, args),
            field
        );

    /// <summary>
    /// Creates a general validation error not associated with a specific field.
    /// </summary>
    /// <param name="message">A human-readable description of the validation failure.</param>
    public static Error Validation(string message) => new(ErrorCodes.Validation, message);

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

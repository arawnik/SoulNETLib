namespace SoulNETLib.Clean.Domain;

/// <summary>
/// A non-generic validation result type that represents a failure and
/// carries one or more <see cref="Error"/> entries.
/// </summary>
/// <remarks>
/// Use <see cref="WithErrors(Error[])"/> to create an instance containing
/// the specific validation errors encountered.
/// </remarks>
public sealed class ValidationResult : Result, IValidationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationResult"/> class
    /// with the specified validation errors.
    /// </summary>
    /// <param name="errors">An array of <see cref="Error"/> describing each validation failure.</param>
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError) => Errors = errors;

    /// <inheritdoc/>
    public IEnumerable<Error> Errors { get; } = [];

    /// <summary>
    /// Creates a <see cref="ValidationResult"/> containing the given validation errors.
    /// </summary>
    /// <param name="errors">An array of <see cref="Error"/> instances to include in the result.</param>
    /// <returns>A new <see cref="ValidationResult"/> populated with the provided errors.</returns>
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

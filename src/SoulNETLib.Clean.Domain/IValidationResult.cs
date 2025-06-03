﻿namespace SoulNETLib.Clean.Domain;

/// <summary>
/// Represents the outcome of a validation operation,
/// encapsulating one or more <see cref="Error"/>s when validation fails.
/// </summary>
public interface IValidationResult
{
    /// <summary>
    /// A default <see cref="Error"/> indicating a general validation failure.
    /// </summary>
    public static readonly Error ValidationError = new(
        ErrorCodes.Validation,
        "A validation problem occured."
    );

    /// <summary>
    /// Gets the collection of validation errors that caused the result to be invalid.
    /// </summary>
    IEnumerable<Error> Errors { get; }
}

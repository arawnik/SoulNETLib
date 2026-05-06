using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Application.Abstractions.Validation;
using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Behaviors;

/// <summary>
/// A pipeline behavior that runs all registered <see cref="ICommandValidator{TCommand}"/>
/// instances before the command handler executes.
/// </summary>
/// <typeparam name="TCommand">The type of command being validated.</typeparam>
/// <remarks>
/// <para>
/// All validators are executed regardless of individual failures — errors are aggregated
/// from every validator into a single <see cref="ValidationResult"/>.
/// </para>
/// <para>
/// When no validators are registered, the behavior is a no-op and calls the next step directly.
/// </para>
/// </remarks>
public sealed class ValidationBehavior<TCommand>(
    IEnumerable<ICommandValidator<TCommand>> validators
) : IPipelineBehavior<TCommand>
    where TCommand : ICommand
{
    private readonly IEnumerable<ICommandValidator<TCommand>> _validators = validators;

    /// <inheritdoc/>
    public async Task<Result> Handle(
        TCommand command,
        PipelineStep next,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(next);

        var errors = await ValidationHelper.CollectErrorsAsync(
            _validators,
            command,
            cancellationToken
        );

        if (errors.Length > 0)
        {
            return ValidationResult.WithErrors(errors);
        }

        return await next();
    }
}

/// <summary>
/// A pipeline behavior that runs all registered <see cref="ICommandValidator{TCommand}"/>
/// instances before the command handler executes (generic result variant).
/// </summary>
/// <typeparam name="TCommand">The type of command being validated.</typeparam>
/// <typeparam name="TResponse">The type of the response value on success.</typeparam>
/// <remarks>
/// <para>
/// All validators are executed regardless of individual failures — errors are aggregated
/// from every validator into a single <see cref="ValidationResult{TResponse}"/>.
/// </para>
/// <para>
/// When no validators are registered, the behavior is a no-op and calls the next step directly.
/// </para>
/// </remarks>
public sealed class ValidationBehavior<TCommand, TResponse>(
    IEnumerable<ICommandValidator<TCommand>> validators
) : IPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly IEnumerable<ICommandValidator<TCommand>> _validators = validators;

    /// <inheritdoc/>
    public async Task<Result<TResponse>> Handle(
        TCommand command,
        PipelineStep<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(next);

        var errors = await ValidationHelper.CollectErrorsAsync(
            _validators,
            command,
            cancellationToken
        );

        if (errors.Length > 0)
        {
            return ValidationResult<TResponse>.WithErrors(errors);
        }

        return await next();
    }
}

/// <summary>
/// Internal helper that collects validation errors from all registered validators.
/// </summary>
internal static class ValidationHelper
{
    internal static async Task<Error[]> CollectErrorsAsync<TCommand>(
        IEnumerable<ICommandValidator<TCommand>> validators,
        TCommand command,
        CancellationToken cancellationToken
    )
        where TCommand : IBaseCommand
    {
        var errorTasks = validators.Select(v => v.ValidateAsync(command, cancellationToken));
        var results = await Task.WhenAll(errorTasks);
        return [.. results.SelectMany(e => e)];
    }
}

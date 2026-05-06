using Microsoft.Extensions.DependencyInjection;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Application.Abstractions.Validation;

namespace SoulNETLib.Clean.Application.DependencyInjection;

/// <summary>
/// Extension methods for registering CQRS command and query handlers with pipeline behavior support.
/// </summary>
/// <remarks>
/// <para>
/// These methods register handlers so that all matching <see cref="IPipelineBehavior{TCommand}"/> or
/// <see cref="IPipelineBehavior{TCommand, TResponse}"/> instances are automatically chained
/// before the handler executes.
/// </para>
/// <para>
/// Pipeline behaviors are resolved from DI as <c>IEnumerable</c> and execute in registration order.
/// If no behaviors are registered, the handler is invoked directly with no overhead.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// // Register a behavior that applies to all commands returning Result
/// services.AddScoped(typeof(IPipelineBehavior&lt;&gt;), typeof(ValidationBehavior&lt;&gt;));
///
/// // Register a handler with pipeline support
/// services.AddCommandHandler&lt;CreateProject.Command, Guid, CreateProject.Handler&gt;();
/// </code>
/// </para>
/// </remarks>
public static class PipelineServiceCollectionExtensions
{
    /// <summary>
    /// Registers a command handler (returning <see cref="Abstractions.CQRS.ICommandHandler{TCommand}"/>)
    /// wrapped in a pipeline that executes all matching <see cref="IPipelineBehavior{TCommand}"/> behaviors.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <typeparam name="THandler">The concrete handler implementation type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCommandHandler<TCommand, THandler>(
        this IServiceCollection services
    )
        where TCommand : ICommand
        where THandler : class, ICommandHandler<TCommand>
    {
        services.AddScoped<THandler>();
        services.AddScoped<ICommandHandler<TCommand>>(sp => new PipelineCommandHandler<TCommand>(
            sp.GetRequiredService<THandler>(),
            sp.GetRequiredService<IEnumerable<IPipelineBehavior<TCommand>>>()
        ));
        return services;
    }

    /// <summary>
    /// Registers a command handler (returning <see cref="Abstractions.CQRS.ICommandHandler{TCommand, TResponse}"/>)
    /// wrapped in a pipeline that executes all matching <see cref="IPipelineBehavior{TCommand, TResponse}"/> behaviors.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <typeparam name="TResponse">The response type returned on success.</typeparam>
    /// <typeparam name="THandler">The concrete handler implementation type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCommandHandler<TCommand, TResponse, THandler>(
        this IServiceCollection services
    )
        where TCommand : ICommand<TResponse>
        where THandler : class, ICommandHandler<TCommand, TResponse>
    {
        services.AddScoped<THandler>();
        services.AddScoped<ICommandHandler<TCommand, TResponse>>(sp => new PipelineCommandHandler<
            TCommand,
            TResponse
        >(
            sp.GetRequiredService<THandler>(),
            sp.GetRequiredService<IEnumerable<IPipelineBehavior<TCommand, TResponse>>>()
        ));
        return services;
    }

    /// <summary>
    /// Registers a query handler wrapped in a pipeline that executes all matching
    /// <see cref="IQueryPipelineBehavior{TQuery, TResponse}"/> behaviors.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    /// <typeparam name="TResponse">The response type returned on success.</typeparam>
    /// <typeparam name="THandler">The concrete handler implementation type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddQueryHandler<TQuery, TResponse, THandler>(
        this IServiceCollection services
    )
        where TQuery : IQuery<TResponse>
        where THandler : class, IQueryHandler<TQuery, TResponse>
    {
        services.AddScoped<THandler>();
        services.AddScoped<IQueryHandler<TQuery, TResponse>>(sp => new PipelineQueryHandler<
            TQuery,
            TResponse
        >(
            sp.GetRequiredService<THandler>(),
            sp.GetRequiredService<IEnumerable<IQueryPipelineBehavior<TQuery, TResponse>>>()
        ));
        return services;
    }

    /// <summary>
    /// Registers a command validator for a specific command type.
    /// The validator will be resolved by <see cref="Behaviors.ValidationBehavior{TCommand}"/>
    /// or <see cref="Behaviors.ValidationBehavior{TCommand, TResponse}"/>.
    /// </summary>
    /// <typeparam name="TCommand">The command type to validate.</typeparam>
    /// <typeparam name="TValidator">The concrete validator implementation type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCommandValidator<TCommand, TValidator>(
        this IServiceCollection services
    )
        where TCommand : IBaseCommand
        where TValidator : class, ICommandValidator<TCommand>
    {
        services.AddScoped<ICommandValidator<TCommand>, TValidator>();
        return services;
    }
}

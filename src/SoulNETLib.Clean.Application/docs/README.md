# SoulNETLib.Clean.Application

Application layer building blocks for Clean Architecture with CQRS. Provides command/query handler interfaces, a pipeline behavior system for cross-cutting concerns, built-in validation behavior, and DI registration extensions.

## Installation

```shell
dotnet add package SoulNETLib.Clean.Application
```

**Requirements:** .NET 10+. Depends on `SoulNETLib.Clean.Domain` and `Microsoft.Extensions.DependencyInjection.Abstractions`.

## Features

- **CQRS interfaces** — `ICommand`, `ICommand<T>`, `IQuery<T>`, `ICommandHandler<>`, `IQueryHandler<>`
- **Pipeline behaviors** — Middleware-style interceptors for commands and queries
- **Validation behavior** — Runs all registered validators before handler execution, aggregates errors
- **DI extensions** — Type-safe registration of handlers, validators, and pipeline wiring

## Quick Start

### Define a command and handler

```csharp
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

public static class CreateProject
{
    public sealed record Command(string Name, string Description) : ICommand<Guid>;

    public sealed class Handler : ICommandHandler<Command, Guid>
    {
        public async Task<Result<Guid>> Handle(Command command, CancellationToken ct)
        {
            var id = Guid.NewGuid();
            // ... persist project
            return Result.Success(id);
        }
    }
}
```

### Define a query and handler

```csharp
public static class GetProjects
{
    public sealed record Query(string? Filter) : IQuery<IReadOnlyList<ProjectModel>>;

    public sealed class Handler : IQueryHandler<Query, IReadOnlyList<ProjectModel>>
    {
        public async Task<Result<IReadOnlyList<ProjectModel>>> Handle(Query query, CancellationToken ct)
        {
            // ... fetch from database
            return Result.Success<IReadOnlyList<ProjectModel>>(projects);
        }
    }
}
```

### Register handlers with pipeline support

```csharp
using SoulNETLib.Clean.Application.DependencyInjection;

services.AddCommandHandler<CreateProject.Command, Guid, CreateProject.Handler>();
services.AddQueryHandler<GetProjects.Query, IReadOnlyList<ProjectModel>, GetProjects.Handler>();
```

### Add validation

Implement `ICommandValidator<TCommand>` for your commands:

```csharp
using SoulNETLib.Clean.Application.Abstractions.Validation;
using SoulNETLib.Clean.Domain;

public sealed class CreateProjectValidator : ICommandValidator<CreateProject.Command>
{
    public Task<Error[]> ValidateAsync(CreateProject.Command command, CancellationToken ct)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(command.Name))
            errors.Add(Error.Validation("name", "Name is required"));

        return Task.FromResult(errors.ToArray());
    }
}
```

Register the validator and enable the validation behavior:

```csharp
using SoulNETLib.Clean.Application.Behaviors;
using SoulNETLib.Clean.Application.DependencyInjection;

// Register validation behavior globally (once)
services.AddScoped(typeof(IPipelineBehavior<>), typeof(ValidationBehavior<>));
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Register validators per command
services.AddCommandValidator<CreateProject.Command, CreateProjectValidator>();
```

All validators for a command run concurrently. Errors are aggregated into a single `ValidationResult` — the handler is never called if validation fails.

### Custom pipeline behaviors

Create cross-cutting behaviors (logging, timing, authorization):

```csharp
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

public sealed class LoggingBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public async Task<Result<TResponse>> Handle(
        TCommand command,
        PipelineStep<TResponse> next,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling {typeof(TCommand).Name}");
        var result = await next();
        Console.WriteLine($"Handled {typeof(TCommand).Name}: {(result.IsSuccess ? "Success" : "Failure")}");
        return result;
    }
}
```

## Architecture

```
┌─────────────────────────────────────────┐
│  Presentation (Web API / Blazor)        │
│  Injects ICommandHandler / IQueryHandler│
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│  Pipeline Behaviors (registered order)  │
│  ValidationBehavior → LoggingBehavior   │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│  Command/Query Handler                  │
│  Business logic execution               │
└─────────────────────────────────────────┘
```

Behaviors execute in registration order (first registered = outermost). If no behaviors are registered, handlers are invoked directly with no overhead.

## Feedback

Found a bug or have a suggestion? [Open an issue](https://github.com/arawnik/SoulNETLib/issues) on GitHub.

## Contributing

Contributions are welcome! Fork the repository, make your changes, and submit a pull request. Please ensure all existing tests pass.

## License

This package is licensed under the [MIT License](https://github.com/arawnik/SoulNETLib/blob/main/LICENSE).

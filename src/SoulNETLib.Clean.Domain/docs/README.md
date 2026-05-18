# SoulNETLib.Clean.Domain

Domain building blocks for Clean Architecture projects. Provides a Result pattern, typed errors with classification codes, validation results, and repository abstractions — all without external dependencies.

## Installation

```shell
dotnet add package SoulNETLib.Clean.Domain
```

**Requirements:** .NET 10+. No external dependencies.

## Features

- **`Result` / `Result<T>`** — Railway-oriented success/failure types with implicit conversions.
- **`Error`** — Sealed record with `Code`, `Message`, and optional `Field` for structured error reporting.
- **`ErrorCodes`** — Standard classification constants (`NotFound`, `Validation`, `BusinessRule`, `Conflict`, etc.).
- **`ValidationResult` / `ValidationResult<T>`** — Aggregates multiple validation errors into a single result.
- **`IUnitOfWork`** — Transaction boundary abstraction for write operations.
- **`IRepository`** — Marker interface for repository pattern implementations.

## Quick Start

### Result pattern

```csharp
using SoulNETLib.Clean.Domain;

public Task<Result<Guid>> CreateProject(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        return Task.FromResult<Result<Guid>>(Error.Validation("name", "Name is required"));

    var id = Guid.NewGuid();
    // ... persist
    return Task.FromResult(Result.Success(id));
}
```

### Error handling

```csharp
using SoulNETLib.Clean.Domain;

// Simple errors
var notFound = Error.NotFound("Recipe", recipeId);     // "Recipe with ID {id} not found."
var validation = Error.Validation("email", "Invalid email format");
var business = Error.BusinessRule("Account balance too low");

// Format template overloads (for localized/parameterized messages)
var notFound2 = Error.NotFound("{0} with ID {1} not found.", entityName, id);
var validation2 = Error.Validation("Name", "'{0}' must be between {1} and {2} characters.", fieldName, 1, 200);
var business2 = Error.BusinessRule("{0} has already been closed.", entityName);

// Errors convert implicitly to Results
Result result = Error.NotFound();
Result<User> userResult = Error.Validation("name", "Required");
```

### Validation results with multiple errors

```csharp
using SoulNETLib.Clean.Domain;

Error[] errors =
[
    Error.Validation("title", "Title is required"),
    Error.Validation("email", "Invalid format"),
];

Result result = ValidationResult.WithErrors(errors);

// Check for validation result specifically
if (result is IValidationResult validation)
{
    foreach (var error in validation.Errors)
        Console.WriteLine($"{error.Field}: {error.Message}");
}
```

### Unit of Work

```csharp
using SoulNETLib.Clean.Domain.Repositories;

public class CreateOrderHandler(IOrderRepository orders, IUnitOfWork unitOfWork)
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken ct)
    {
        var order = new Order(command.CustomerId, command.Items);
        orders.Add(order);
        await unitOfWork.CompleteAsync(ct);
        return Result.Success(order.Id);
    }
}
```

## Error Factory Methods

| Method | Field | Use case |
|--------|-------|----------|
| `Error.NotFound(message?)` | — | Generic not-found with optional message |
| `Error.NotFound(template, params args)` | — | Formatted not-found (localization-friendly) |
| `Error.NotFound<TKey>(type, key)` | — | Type+key not-found ("Recipe with ID {key} not found.") |
| `Error.Validation(field, message)` | ✓ | Field-specific validation error |
| `Error.Validation(field, template, params args)` | ✓ | Formatted field validation (localization-friendly) |
| `Error.Validation(message)` | — | General validation error (no field) |
| `Error.BusinessRule(message)` | — | Domain rule violation |
| `Error.BusinessRule(template, params args)` | — | Formatted business rule (localization-friendly) |
| `Error.InvalidData(message)` | — | Data processing error |
| `Error.FromException(ex, code?)` | — | Convert exception to error |

The `template + params args` overloads use `string.Format(CultureInfo.InvariantCulture, ...)` internally, making them ideal for resource-file-based localization patterns.

## Error Codes

| Constant | Typical HTTP mapping | Use case |
|----------|---------------------|----------|
| `ErrorCodes.NotFound` | 404 | Entity not found |
| `ErrorCodes.Validation` | 400 | Input validation failure |
| `ErrorCodes.BusinessRule` | 422 | Domain rule violation |
| `ErrorCodes.Conflict` | 409 | Duplicate or state conflict |
| `ErrorCodes.Unauthorized` | 401 | Authentication required |
| `ErrorCodes.InvalidData` | 500 | Data processing error |

## Feedback

Found a bug or have a suggestion? [Open an issue](https://github.com/arawnik/SoulNETLib/issues) on GitHub.

## Contributing

Contributions are welcome! Fork the repository, make your changes, and submit a pull request. Please ensure all existing tests pass.

## License

This package is licensed under the [MIT License](https://github.com/arawnik/SoulNETLib/blob/main/LICENSE).

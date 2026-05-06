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

// Create typed errors
var notFound = Error.NotFound<Guid>(entityId);
var validation = Error.Validation("email", "Invalid email format");
var business = Error.BusinessRule("InsufficientFunds", "Account balance too low");

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

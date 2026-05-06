# SoulNETLib.Clean.Infrastructure

Infrastructure layer package for Clean Architecture projects. Provides a foundation for implementing application ports (repositories, external services, query implementations) in the outermost layer.

## Installation

```shell
dotnet add package SoulNETLib.Clean.Infrastructure
```

**Requirements:** .NET 10+.

## Purpose

This package serves as the infrastructure layer in a Clean Architecture stack:

```
SoulNETLib.Clean.Domain          ← No dependencies (entities, Result, Error)
SoulNETLib.Clean.Application     ← Depends on Domain (handlers, ports, behaviors)
SoulNETLib.Clean.Infrastructure  ← Implements Application ports (EF Core, HTTP clients, etc.)
```

Use this package as a base for your infrastructure implementations — repositories, query port implementations, external service adapters, and other I/O concerns.

## Planned Features

- Base repository implementations for EF Core
- Common infrastructure patterns and helpers

## Related Packages

| Package | Purpose |
|---------|---------|
| [SoulNETLib.Clean.Domain](https://www.nuget.org/packages/SoulNETLib.Clean.Domain) | Domain primitives (Result, Error, repository interfaces) |
| [SoulNETLib.Clean.Application](https://www.nuget.org/packages/SoulNETLib.Clean.Application) | CQRS handlers, pipeline behaviors, validation |
| [SoulNETLib.EFCore](https://www.nuget.org/packages/SoulNETLib.EFCore) | EF Core pagination utilities |

## Feedback

Found a bug or have a suggestion? [Open an issue](https://github.com/arawnik/SoulNETLib/issues) on GitHub.

## Contributing

Contributions are welcome! Fork the repository, make your changes, and submit a pull request. Please ensure all existing tests pass.

## License

This package is licensed under the [MIT License](https://github.com/arawnik/SoulNETLib/blob/main/LICENSE).

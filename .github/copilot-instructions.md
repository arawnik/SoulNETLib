# Copilot Instructions — SoulNETLib

SoulNETLib is a public NuGet package library. Optimize for API clarity, backward compatibility, comprehensive documentation, and test coverage.

## Repository structure

```
src/
  SoulNETLib.Clean.Domain/        — Result pattern, Error, ValidationResult, repository abstractions
  SoulNETLib.Clean.Application/   — CQRS interfaces, pipeline behaviors, validation, DI extensions
  SoulNETLib.Clean.Infrastructure/— Infrastructure helpers (currently minimal)
  SoulNETLib.Common/              — General-purpose extension methods
  SoulNETLib.EFCore/              — EF Core extension methods and collection helpers
test/
  SoulNETLib.Clean.Application.Tests/
  SoulNETLib.EFCore.Tests/
  SoulNETLib.Tests/
  SoulNETLibTests.Common/         — Shared test utilities
```

## Core design principles

- **Library-first mindset** — every public API must be designed for external consumption. Favor discoverability, minimal surprises, and XML doc comments on all public members.
- **Zero external dependencies where possible** — `Clean.Domain` has no dependencies. `Clean.Application` depends only on `Clean.Domain` and `Microsoft.Extensions.DependencyInjection.Abstractions`.
- **Backward compatibility** — avoid breaking changes. Use obsolete attributes before removing APIs. Bump major version for breaking changes.
- **Strict analysis** — `TreatWarningsAsErrors`, `AnalysisMode=All`, `EnforceCodeStyleInBuild`. All code must compile warning-free.

## Package-specific conventions

### SoulNETLib.Clean.Domain
- Contains the `Result`/`Result<T>` pattern, `Error`, `ValidationResult`, `IValidationResult`.
- `Error` uses a `Code` string (from `ErrorCodes` constants) plus `Message` and optional `Field`.
- `ValidationResult` / `ValidationResult<T>` implement `IValidationResult` and carry `Error[]`.
- Repository abstractions: `IEntityRepository<T>`, `IUnitOfWork`.
- No dependencies — this package must remain dependency-free.

### SoulNETLib.Clean.Application
- **CQRS interfaces**: `ICommand`, `ICommand<T>`, `IQuery<T>`, `ICommandHandler<>`, `IQueryHandler<>`.
- **Pipeline behaviors**: `IPipelineBehavior<TCommand>` (non-generic result) and `IPipelineBehavior<TCommand, TResponse>` (generic result). Behaviors wrap handler execution like middleware.
- **ValidationBehavior**: Built-in behavior that resolves all `ICommandValidator<T>` for a command, runs them concurrently, aggregates errors, and short-circuits with `ValidationResult` if any fail.
- **ValidationHelper**: Internal static helper used by both `ValidationBehavior<>` variants to collect errors from validators.
- **DI extensions** (`PipelineServiceCollectionExtensions`):
  - `AddCommandHandler<TCommand, THandler>()` — registers handler wrapped in pipeline dispatch.
  - `AddCommandHandler<TCommand, TResponse, THandler>()` — generic variant.
  - `AddQueryHandler<TQuery, TResponse, THandler>()` — query registration.
  - `AddCommandValidator<TCommand, TValidator>()` — registers validator as `ICommandValidator<TCommand>`.
- All handler registrations create a pipeline-aware delegate that resolves behaviors at runtime.

### SoulNETLib.Common
- Pure extension methods (`string`, collections, etc.).
- No dependencies on other SoulNETLib packages.

### SoulNETLib.EFCore
- EF Core-specific extensions (e.g., `LeftJoin`).
- Collection helpers.

## NuGet package documentation

Each package has a `docs/README.md` that is packed into the `.nupkg` (configured via `Directory.Build.props`).

**When modifying a package's public API or behavior:**
1. Update the corresponding `docs/README.md` to reflect changes.
2. Keep examples working and up-to-date.
3. Include: what it does, installation, quick start with code samples, architecture notes where relevant.

## Versioning

- Version is centralized in `Directory.Build.props` (`<Version>` property).
- All packages share the same version number.
- Bump version when publishing changes:
  - Patch: bug fixes, internal refactors, doc updates.
  - Minor: new features, new public APIs.
  - Major: breaking changes.

## Testing

- Every public API and behavior must have corresponding tests.
- Test projects mirror src projects: `SoulNETLib.Clean.Application.Tests`, etc.
- Use `InternalsVisibleTo` (configured in `Directory.Build.props`) to test internal implementation details.
- All tests must pass before any changes are considered complete.
- Prefer descriptive test names: `MethodName_Scenario_ExpectedResult` or `MethodName_ReturnsExpectedResult_WhenCondition`.

## Code style

- XML doc comments on all `public` and `protected` members.
- `internal` for implementation details that shouldn't be part of the public API surface.
- `sealed` classes by default unless extension is an explicit design goal.
- Favor explicitness over cleverness — library consumers read this code.
- Keep files focused: one primary type per file.
- Use file-scoped namespaces.

## Directory.Build.props conventions

Centralized in root `Directory.Build.props`:
- `TargetFramework`, `ImplicitUsings`, `Nullable`, analysis settings.
- `Version`, `Authors`, `Company`, `RepositoryUrl`.
- `PackageLicenseExpression` (MIT).
- `PackageReadmeFile` + conditional ItemGroup for `docs/README.md`.
- Global `InternalsVisibleTo` pattern using `$(MSBuildProjectName).Tests`.

## When generating new code, Copilot must

- Ensure all public APIs have XML doc comments.
- Add or update tests for any new or changed behavior.
- Update `docs/README.md` if the package's public API or usage patterns change.
- Never introduce dependencies that would leak into consuming projects unexpectedly.
- Keep the pipeline behavior system symmetric (both generic and non-generic variants must be updated together).
- Verify all 5 packages build warning-free with `TreatWarningsAsErrors`.

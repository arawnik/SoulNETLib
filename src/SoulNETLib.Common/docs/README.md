# SoulNETLib.Common

A lightweight collection of extension methods for everyday .NET development. Reduces boilerplate across strings, enums, collections, claims, and more.

## Installation

```shell
dotnet add package SoulNETLib.Common
```

**Requirements:** .NET 10+

## What's Included

| Category | Key Methods |
|----------|-------------|
| **String** | `OrEmptyIfNull`, `RemoveWhitespaces`, `TryRemoveEnd`, `TryRemoveStart`, `FirstCharToUpper`, `GetDigitsInt` |
| **Enum** | `GetDescription`, `GetEnumMember`, `TryParseEnumMember` |
| **IEnumerable** | `OrEmptyIfNull`, `WhereIf` |
| **IQueryable** | `WhereIf` (expression tree variant) |
| **List** | `Replace`, `ReplaceAll` |
| **Object** | `HasProperty`, `CopyPropertyValuesFrom` |
| **Claims** | `GetUserId` |
| **Exception** | `GetDetailMessage` |

## Quick Start

### Conditional filtering

```csharp
using SoulNETLib.Common.Extension;

// Apply filter only when a value is provided — works with IQueryable and IEnumerable
var results = query
    .WhereIf(!string.IsNullOrEmpty(searchTerm), x => x.Name.Contains(searchTerm))
    .WhereIf(categoryId.HasValue, x => x.CategoryId == categoryId);
```

### Enum utilities

```csharp
using SoulNETLib.Common.Extension;

public enum Status
{
    [Description("In Progress")]
    InProgress,

    [EnumMember(Value = "done")]
    Completed
}

var description = Status.InProgress.GetDescription(); // "In Progress"
var member = Status.Completed.GetEnumMember();         // "done"

if ("done".TryParseEnumMember<Status>(out var parsed))
{
    // parsed == Status.Completed
}
```

### Null-safe collections

```csharp
using SoulNETLib.Common.Extension;

IEnumerable<string>? possiblyNull = GetItems();
var safe = possiblyNull.OrEmptyIfNull(); // never null

string? name = GetName();
var safeName = name.OrEmptyIfNull(); // ""
```

## Feedback

Found a bug or have a suggestion? [Open an issue](https://github.com/arawnik/SoulNETLib/issues) on GitHub.

## Contributing

Contributions are welcome! Fork the repository, make your changes, and submit a pull request. Please ensure all existing tests pass.

## License

This package is licensed under the [MIT License](https://github.com/arawnik/SoulNETLib/blob/main/LICENSE).

# SoulNETLib.EFCore

Pagination support for Entity Framework Core. Provides `PaginatedList<T>` and `PaginatedResult<T>` with IQueryable extension methods for efficient server-side paging.

## Installation

```shell
dotnet add package SoulNETLib.EFCore
```

**Requirements:** .NET 10+, Entity Framework Core

## Features

- **`PaginatedList<T>`** — Read-only collection with full pagination metadata (page count, row count, has previous/next).
- **`PaginatedResult<T>`** — JSON-serializable wrapper for API responses with ordered property serialization.
- **IQueryable extensions** — `ToPaginatedListAsync` and `ToPaginatedResultAsync` for one-liner pagination.

## Quick Start

### Paginate a query

```csharp
using SoulNETLib.EFCore.Extension;

var pagedItems = await dbContext.Products
    .Where(p => p.IsActive)
    .OrderBy(p => p.Name)
    .ToPaginatedListAsync(currentPage: 1, pageSize: 20);

// Access metadata
Console.WriteLine($"Page {pagedItems.CurrentPage} of {pagedItems.PageCount}");
Console.WriteLine($"Total rows: {pagedItems.RowCount}");
Console.WriteLine($"Has next: {pagedItems.HasNextPage}");
```

### Return paginated result from an API

```csharp
using SoulNETLib.EFCore.Extension;

var result = await dbContext.Orders
    .OrderByDescending(o => o.CreatedAt)
    .ToPaginatedResultAsync(page, pageSize);

// PaginatedResult<T> is ready for JSON serialization
return Results.Ok(result);
```

### Pagination metadata

Both `PaginatedList<T>` and `PaginatedResult<T>` expose:

| Property | Description |
|----------|-------------|
| `CurrentPage` | Current page number (1-based) |
| `PageSize` | Items per page |
| `PageCount` | Total number of pages |
| `RowCount` | Total number of items |
| `HasPreviousPage` | Whether a previous page exists |
| `HasNextPage` | Whether a next page exists |

## Feedback

Found a bug or have a suggestion? [Open an issue](https://github.com/arawnik/SoulNETLib/issues) on GitHub.

## Contributing

Contributions are welcome! Fork the repository, make your changes, and submit a pull request. Please ensure all existing tests pass.

## License

This package is licensed under the [MIT License](https://github.com/arawnik/SoulNETLib/blob/main/LICENSE).

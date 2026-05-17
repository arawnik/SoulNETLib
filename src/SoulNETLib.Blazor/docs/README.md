# SoulNETLib.Blazor

Common Blazor utilities for form validation, field identifier resolution, and CSS framework integration. Designed as a shared foundation for Blazor applications using SoulNETLib.

## Installation

```bash
dotnet add package SoulNETLib.Blazor
```

## Utilities

### FieldIdentifierHelper

Static utility for resolving dotted property paths to Blazor `FieldIdentifier` instances. Supports simple properties, nested objects, indexed collections, and case-insensitive matching. Framework-agnostic — works with any CSS framework.

```csharp
// "Ingredients[0].Amount" → FieldIdentifier pointing to the Amount property
// on the first item in the Ingredients collection
var fieldId = FieldIdentifierHelper.ToFieldIdentifier(editContext, "Ingredients[0].Amount");
```

## Bootstrap Integration

### BootstrapFieldCssClassProvider

Maps Blazor validation state to Bootstrap CSS classes (`is-valid` / `is-invalid`). Unmodified fields show no styling; modified fields show green or red borders.

```csharp
var editContext = new EditContext(model);
editContext.SetFieldCssClassProvider(new BootstrapFieldCssClassProvider());
```

> **Note:** `BootstrapValidation` sets this automatically — you only need to set it manually when not using the validation component.

### BootstrapValidation

A Blazor validator component that pushes server-side validation errors into the `EditContext`, styled for Bootstrap forms.

**Razor usage:**

```razor
<EditForm Model="@model" OnValidSubmit="HandleSubmit">
    <DataAnnotationsValidator />
    <BootstrapValidation @ref="serverValidation" />

    <InputText @bind-Value="model.Name" class="form-control" />
    <ValidationMessage For="() => model.Name" />

    <button type="submit" class="btn btn-primary">Save</button>
</EditForm>
```

**Pushing server errors (Blazor WASM with HTTP API):**

```csharp
private BootstrapValidation? serverValidation;

private async Task HandleSubmit()
{
    var response = await Http.PostAsJsonAsync("api/items", model);

    if (!response.IsSuccessStatusCode)
    {
        // Parse your error response into Dictionary<string, IReadOnlyList<string>>
        var errors = await ParseErrors(response);
        serverValidation?.DisplayErrors(errors);
    }
}
```

**Pushing server errors (Server-side Blazor with direct handler injection):**

```csharp
private BootstrapValidation? serverValidation;

private async Task HandleSubmit()
{
    var result = await handler.Handle(command, cancellationToken);

    if (result is IValidationResult validation)
    {
        var errors = validation.Errors
            .GroupBy(e => e.Field ?? string.Empty)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<string>)g.Select(e => e.Message).ToList());

        serverValidation?.DisplayErrors(errors);
    }
}
```

## Related Packages

| Package | Purpose |
|---------|---------|
| [SoulNETLib.Clean.Domain](https://www.nuget.org/packages/SoulNETLib.Clean.Domain) | Result pattern, Error type, repository abstractions |
| [SoulNETLib.Clean.Application](https://www.nuget.org/packages/SoulNETLib.Clean.Application) | CQRS interfaces, pipeline behaviors, validation |

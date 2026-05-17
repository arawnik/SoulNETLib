using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace SoulNETLib.Blazor.Bootstrap;

/// <summary>
/// A reusable Blazor validator component that pushes server-side validation errors
/// into the cascaded <see cref="EditContext"/> via a <see cref="ValidationMessageStore"/>,
/// styled for Bootstrap forms.
/// </summary>
/// <remarks>
/// <para>
/// Place this component inside an <see cref="EditForm"/> alongside (or instead of)
/// <c>DataAnnotationsValidator</c>. After a server round-trip returns validation errors,
/// call <see cref="DisplayErrors"/> to show them on the matching fields.
/// </para>
/// <para>
/// The component automatically:
/// <list type="bullet">
///   <item>Sets <see cref="BootstrapFieldCssClassProvider"/> on the <see cref="EditContext"/>
///         so that form fields render Bootstrap <c>is-valid</c>/<c>is-invalid</c> classes.</item>
///   <item>Clears all server errors when a new validation pass is requested
///         (<see cref="EditContext.OnValidationRequested"/>).</item>
///   <item>Clears individual field errors when the user modifies that field
///         (<see cref="EditContext.OnFieldChanged"/>).</item>
/// </list>
/// </para>
/// <para>Supported field name patterns (via dictionary keys):</para>
/// <list type="bullet">
///   <item><c>"Name"</c> — simple property on the root model.</item>
///   <item><c>"Ingredients[0].Amount"</c> — indexed collection item property.</item>
///   <item><c>"Address.City"</c> — nested object property.</item>
///   <item><c>""</c> — model-level error, shown by <c>ValidationSummary</c>.</item>
/// </list>
/// </remarks>
public class BootstrapValidation : ComponentBase, IDisposable
{
    private ValidationMessageStore? messageStore;

    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(
                $"{nameof(BootstrapValidation)} requires a cascading "
                    + $"parameter of type {nameof(EditContext)}. "
                    + $"For example, you can use {nameof(BootstrapValidation)} "
                    + $"inside an {nameof(EditForm)}."
            );
        }

        messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.SetFieldCssClassProvider(new BootstrapFieldCssClassProvider());

        CurrentEditContext.OnValidationRequested += OnValidationRequested;
        CurrentEditContext.OnFieldChanged += OnFieldChanged;
    }

    /// <summary>
    /// Adds server validation errors to the form, resolving field names to the
    /// correct <see cref="FieldIdentifier"/> on the <see cref="EditContext.Model"/>.
    /// </summary>
    /// <param name="errors">
    /// A dictionary where keys are field paths (e.g. <c>"Name"</c>, <c>"Items[0].Title"</c>)
    /// and values are the error messages for that field.
    /// An empty key represents a model-level error.
    /// </param>
    public void DisplayErrors(Dictionary<string, IReadOnlyList<string>> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        if (CurrentEditContext is null || messageStore is null)
            return;

        foreach (var (fieldPath, messages) in errors)
        {
            var fieldIdentifier = FieldIdentifierHelper.ToFieldIdentifier(
                CurrentEditContext,
                fieldPath
            );
            messageStore.Add(fieldIdentifier, messages);
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    /// <summary>
    /// Adds a single model-level error message to the form.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    public void AddModelError(string message)
    {
        if (CurrentEditContext is not null)
        {
            messageStore?.Add(new FieldIdentifier(CurrentEditContext.Model, string.Empty), message);
            CurrentEditContext.NotifyValidationStateChanged();
        }
    }

    /// <summary>
    /// Clears all server-side validation messages.
    /// </summary>
    public void ClearErrors()
    {
        messageStore?.Clear();
        CurrentEditContext?.NotifyValidationStateChanged();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the managed resources used by the <see cref="BootstrapValidation"/>.
    /// Override in derived classes to add custom cleanup logic.
    /// </summary>
    /// <param name="disposing"><c>true</c> when called from <see cref="Dispose()"/>; <c>false</c> from a finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && CurrentEditContext is not null)
        {
            CurrentEditContext.OnValidationRequested -= OnValidationRequested;
            CurrentEditContext.OnFieldChanged -= OnFieldChanged;
        }
    }

    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        messageStore?.Clear();
    }

    private void OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        messageStore?.Clear(e.FieldIdentifier);
    }
}

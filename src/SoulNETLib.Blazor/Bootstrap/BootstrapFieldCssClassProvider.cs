using Microsoft.AspNetCore.Components.Forms;

namespace SoulNETLib.Blazor.Bootstrap;

/// <summary>
/// Maps Blazor's built-in validation state to Bootstrap's
/// <c>is-valid</c> / <c>is-invalid</c> CSS classes so that
/// <see cref="InputBase{TValue}"/> components render correctly
/// inside Bootstrap-styled forms.
/// </summary>
/// <remarks>
/// <para>
/// Unmodified fields show no validation styling.
/// Modified fields show <c>is-valid</c> (green outline) or <c>is-invalid</c> (red outline).
/// </para>
/// <para>
/// Register this provider on an <see cref="EditContext"/> via
/// <c>EditContext.SetFieldCssClassProvider</c>. The <see cref="BootstrapValidation"/>
/// component sets it automatically.
/// </para>
/// </remarks>
public sealed class BootstrapFieldCssClassProvider : FieldCssClassProvider
{
    /// <inheritdoc />
    public override string GetFieldCssClass(
        EditContext editContext,
        in FieldIdentifier fieldIdentifier
    )
    {
        ArgumentNullException.ThrowIfNull(editContext);

        if (!editContext.IsModified(fieldIdentifier))
            return editContext.IsValid(fieldIdentifier) ? string.Empty : "is-invalid";

        return editContext.IsValid(fieldIdentifier) ? "is-valid" : "is-invalid";
    }
}

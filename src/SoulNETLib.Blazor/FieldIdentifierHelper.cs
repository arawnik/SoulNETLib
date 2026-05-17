using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Components.Forms;

namespace SoulNETLib.Blazor;

/// <summary>
/// Provides a utility method that resolves a dotted property path string
/// (e.g. <c>"Ingredients[0].Amount"</c>) into a Blazor <see cref="FieldIdentifier"/>.
/// </summary>
/// <remarks>
/// <para>
/// Server-side validation errors typically arrive as field-path strings.
/// Blazor's <see cref="EditContext"/> requires <see cref="FieldIdentifier"/>
/// instances to display those errors on the correct form fields.
/// This helper bridges that gap by walking the model's object graph
/// via reflection.
/// </para>
/// <para>Supported path patterns:</para>
/// <list type="bullet">
///   <item><c>"Name"</c> — simple property on the root model.</item>
///   <item><c>"Ingredients[0].Amount"</c> — indexed collection item property.</item>
///   <item><c>"Address.City"</c> — nested object property.</item>
///   <item><c>""</c> — model-level (empty path returns identifier on the model itself).</item>
/// </list>
/// <para>
/// Property names are matched <b>case-insensitively</b> so that server field names
/// (e.g. <c>"name"</c>) resolve to PascalCase model properties (e.g. <c>Name</c>).
/// </para>
/// </remarks>
public static class FieldIdentifierHelper
{
    private static readonly char[] Separators = ['.', '['];

    private const BindingFlags PropertyFlags =
        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

    /// <summary>
    /// Walks a dotted property path starting from <see cref="EditContext.Model"/>
    /// and returns a <see cref="FieldIdentifier"/> pointing at the deepest
    /// reachable (object, property) pair.
    /// </summary>
    /// <param name="editContext">The <see cref="EditContext"/> whose <see cref="EditContext.Model"/> is the root object.</param>
    /// <param name="propertyPath">
    /// A dotted/indexed property path such as <c>"Address.City"</c>
    /// or <c>"Items[0].Name"</c>. An empty string returns an identifier for the model itself.
    /// </param>
    /// <returns>
    /// A <see cref="FieldIdentifier"/> resolved to the deepest reachable object in the path.
    /// If traversal encounters a <c>null</c> intermediate, the identifier stops at the last
    /// non-null object.
    /// </returns>
    public static FieldIdentifier ToFieldIdentifier(EditContext editContext, string propertyPath)
    {
        ArgumentNullException.ThrowIfNull(editContext);

        var obj = editContext.Model;

        while (true)
        {
            var nextTokenEnd = propertyPath.IndexOfAny(Separators);
            if (nextTokenEnd < 0)
            {
                // Final segment — resolve to the actual property name for correct casing.
                var finalProp = obj.GetType().GetProperty(propertyPath, PropertyFlags);
                return new FieldIdentifier(obj, finalProp?.Name ?? propertyPath);
            }

            var nextToken = propertyPath[..nextTokenEnd];
            propertyPath = propertyPath[(nextTokenEnd + 1)..];

            object? newObj;
            if (nextToken.EndsWith(']'))
            {
                // Indexer access (e.g. "Items[0]" → token is "0]")
                // Assumes C# convention: one indexer named "Item" with one parameter.
                nextToken = nextToken[..^1];
                var prop = obj.GetType().GetProperty("Item");
                var indexerType = prop?.GetIndexParameters()[0].ParameterType;

                var indexerValue = indexerType is not null
                    ? Convert.ChangeType(nextToken, indexerType, CultureInfo.InvariantCulture)
                    : null;

                newObj = prop?.GetValue(obj, [indexerValue]);
            }
            else
            {
                // Regular property — case-insensitive lookup
                var prop = obj.GetType().GetProperty(nextToken, PropertyFlags);
                newObj = prop?.GetValue(obj);
            }

            if (newObj is null)
            {
                // Stop at the last valid non-null object.
                return new FieldIdentifier(obj, nextToken);
            }

            obj = newObj;
        }
    }
}

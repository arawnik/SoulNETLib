using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace SoulNETLib.Common.Extension;

/// <summary>
/// Provides extension methods for working with <see cref="Enum"/> types.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Retrieves the <see cref="DescriptionAttribute.Description"/> of an enum member, if defined.
    /// Falls back to the enum member name if no description is available.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="source">The enum value to retrieve the description for.</param>
    /// <returns>The description string, or the enum name if no description is found.</returns>
    public static string GetDescription<T>(this T source)
        where T : struct, Enum
    {
        var type = typeof(T);
        var name = Enum.GetName(type, source);

        if (name is null)
            return source.ToString();

        var field = type.GetField(name);
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();

        return attr?.Description ?? name;
    }

    /// <summary>
    /// Retrieves the <see cref="EnumMemberAttribute.Value"/> for a given enum value, if defined.
    /// Falls back to the enum name if no attribute is found.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="source">The enum value to retrieve the EnumMember value for.</param>
    /// <returns>The EnumMember value if defined; otherwise, the enum name.</returns>
    public static string GetEnumMember<T>(this T source)
        where T : struct, Enum
    {
        var type = typeof(T);
        var name = Enum.GetName(type, source);

        if (name is null)
            return source.ToString();

        var field = type.GetField(name);
        var attr = field?.GetCustomAttribute<EnumMemberAttribute>();

        return attr?.Value ?? name;
    }

    /// <summary>
    /// Attempts to parse a string value into an enum member based on its <see cref="EnumMemberAttribute.Value"/>.
    /// The comparison is case-insensitive.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to parse into.</typeparam>
    /// <param name="value">The string value to parse (typically from serialized input).</param>
    /// <param name="result">When successful, contains the parsed enum value.</param>
    /// <returns><c>true</c> if a match was found; otherwise, <c>false</c>.</returns>
    public static bool TryParseEnumMember<TEnum>(this string value, out TEnum? result)
        where TEnum : struct, Enum
    {
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attr = field.GetCustomAttribute<EnumMemberAttribute>();
            if (attr?.Value?.Equals(value, StringComparison.OrdinalIgnoreCase) == true)
            {
                result = (TEnum)field.GetValue(null)!;
                return true;
            }
        }

        result = default;
        return false;
    }
}

namespace SoulNETLib.Common.Extension;

/// <summary>
/// Define extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Make sure that <see cref="string"/> is not null. Allocate empty string if <paramref name="source"/> is null.
    /// </summary>
    /// <param name="source">Instance of the <see cref="string"/> that is being checked for null.</param>
    /// <returns>The original <see cref="string"/>, or empty if original value was null.</returns>
    public static string OrEmptyIfNull(this string? source)
    {
        return source ?? string.Empty;
    }

    /// <summary>
    /// Remove all whitespaces from the <paramref name="source"/> <see cref="string"/>.
    /// </summary>
    /// <param name="source">The <see cref="string"/> of which whitespaces will be removed.</param>
    /// <returns>The <paramref name="source"/> <see cref="string"/> with whitespaces removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
    public static string RemoveWhitespaces(this string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return string.Join(
            "",
            source.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)
        );
    }

    /// <summary>
    /// Remove <paramref name="endStr"/> from the end of <paramref name="source"/>. A return value indicates if removal succeeded.
    /// Also get
    /// </summary>
    /// <param name="source">The <see cref="string"/> from which <paramref name="endStr"/> is removed.</param>
    /// <param name="endStr">The <see cref="string"/> that will be searched from the end of <paramref name="source"/>.</param>
    /// <param name="result"><paramref name="source"/> without <paramref name="endStr"/> at the end if it existed. Original <paramref name="source"/> otherwise.</param>
    /// <returns>true if <paramref name="source"/> ends with <paramref name="endStr"/>. false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> or <paramref name="endStr"/> is null.</exception>
    public static bool TryRemoveEnd(this string source, string endStr, out string result)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(endStr);

        bool ret = source.EndsWith(endStr, StringComparison.Ordinal);
        result = ret ? source[..^endStr.Length] : source;
        return ret;
    }

    /// <summary>
    /// Remove <paramref name="startStr"/> from the start of <paramref name="source"/>. A return value indicates if removal succeeded.
    /// </summary>
    /// <param name="source">The <see cref="string"/> from which <paramref name="startStr"/> is removed.</param>
    /// <param name="startStr">The <see cref="string"/> that will be searched from the start of <paramref name="source"/>.</param>
    /// <param name="result"><paramref name="source"/> without <paramref name="startStr"/> at the start if it existed.</param>
    /// <returns>true if <paramref name="source"/> starts with <paramref name="startStr"/>. false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> or <paramref name="startStr"/> is null.</exception>
    public static bool TryRemoveStart(this string source, string startStr, out string result)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(startStr);

        bool ret = source.StartsWith(startStr, StringComparison.Ordinal);
        result = ret ? source[startStr.Length..] : source;
        return ret;
    }

    /// <summary>
    /// Turn first character of <paramref name="source"/> to upper case.
    /// </summary>
    ///  <param name="source">The <see cref="string"/> from which first character will be modified.</param>
    /// <returns><paramref name="source"/> with first character in upper case.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
    public static string FirstCharToUpper(this string source)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (string.IsNullOrEmpty(source))
            return source;
        return string.Concat(source[0].ToString().ToUpperInvariant(), source.AsSpan(1));
    }

    /// <summary>
    /// Get just Digits from <paramref name="source"/> and return as <see cref="int"/>.
    /// </summary>
    /// <param name="source">The <see cref="string"/> from which we parse.</param>
    /// <returns>parsed number from digits of <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
    public static int GetDigitsInt(this string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var cleared = new String([.. source.Where(Char.IsDigit)]);
        if (int.TryParse(cleared, out var parsedInt))
        {
            return parsedInt;
        }
        return 0;
    }

    /// <summary>
    /// Get just Digits from <paramref name="source"/> and return as <see cref="long"/>.
    /// </summary>
    /// <param name="source">The <see cref="string"/> from which we parse.</param>
    /// <returns>parsed number from digits of <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
    public static long GetDigitsLong(this string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var cleared = new String([.. source.Where(Char.IsDigit)]);
        if (long.TryParse(cleared, out var parsedInt))
        {
            return parsedInt;
        }
        return 0;
    }
}

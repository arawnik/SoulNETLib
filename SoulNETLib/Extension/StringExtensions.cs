using System;

namespace SoulNETLib.Extension
{
    /// <summary>
    /// Define extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Make sure that <see cref="string"/> is not <see cref="null"/>. Allocate empty string if <paramref name="source"/> is <see cref="null"/>.
        /// </summary>
        /// <param name="source">Instance of the <see cref="string"/> that is being checked for <see cref="null"/>.</param>
        /// <returns>The original <see cref="string"/>, or empty if original value was <see cref="null"/>.</returns>
        public static string OrEmptyIfNull(this string? source)
        {
            return source ?? string.Empty;
        }

        /// <summary>
        /// Remove all whitespaces from the <paramref name="source"/> <see cref="string"/>.
        /// </summary>
        /// <param name="source">The <see cref="string"/> of which whitespaces will be removed.</param>
        /// <returns>The <paramref name="source"/> <see cref="string"/> with whitespaces removed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see cref="null"/></exception>
        public static string RemoveWhitespaces(this string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return string.Join("", source.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Remove <paramref name="endStr"/> from the end of <paramref name="source"/>. A return value indicates if removal succeeded.
        /// Also get 
        /// </summary>
        /// <param name="source">The <see cref="string"/> from which <paramref name="endStr"/> is removed.</param>
        /// <param name="endStr">The <see cref="string"/> that will be searched from the end of <paramref name="source"/>.</param>
        /// <param name="result"><paramref name="source"/> without <paramref name="endStr"/> at the end if it existed. Original <paramref name="source"/> otherwise.</param>
        /// <returns><see cref="true"/> if <paramref name="source"/> ends with <paramref name="endStr"/>. <see cref="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> or <paramref name="endStr"/> is <see cref="null"/></exception>
        public static bool TryRemoveEnd(this string source, string endStr, out string result)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (endStr == null)
                throw new ArgumentNullException(nameof(endStr));

            bool ret = source.EndsWith(endStr);
            result = ret ? source[..^endStr.Length] : source;
            return ret;
        }

        /// <summary>
        /// Remove <paramref name="startStr"/> from the start of <paramref name="source"/>. A return value indicates if removal succeeded.
        /// </summary>
        /// <param name="source">The <see cref="string"/> from which <paramref name="startStr"/> is removed.</param>
        /// <param name="startStr">The <see cref="string"/> that will be searched from the start of <paramref name="source"/>.</param>
        /// <param name="result"><paramref name="source"/> without <paramref name="startStr"/> at the start if it existed.</param>
        /// <returns><see cref="true"/> if <paramref name="source"/> starts with <paramref name="startStr"/>. <see cref="false"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> or <paramref name="startStr"/> is <see cref="null"/></exception>
        public static bool TryRemoveStart(this string source, string startStr, out string result)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (startStr == null)
                throw new ArgumentNullException(nameof(startStr));

            bool ret = source.StartsWith(startStr);
            result = ret ? source[startStr.Length..] : source;
            return ret;
        }
    }
}

using System;

namespace SoulNETLib.Extension
{
    /// <summary>
    /// Define extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static  class IEnumerableExtensions
    {
        /// <summary>
        /// Make sure that <see cref="IEnumerable{T}"/> is not <see cref="null"/>. Allocate empty instance if <paramref name="source"/> is <see cref="null"/>.
        /// </summary>
        /// <typeparam name="T">Type of the class that <see cref="IEnumerable{T}"/> enumerates.</typeparam>
        /// <param name="source">Instance of the <see cref="IEnumerable{T}"/> that is being checked for <see cref="null"/>.</param>
        /// <returns>The original <see cref="IEnumerable{T}"/>, or empty if original value was <see cref="null"/>.</returns>
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T>? source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Apply the Where-clause into <paramref name="source"/> if <paramref name="apply"/> is <see cref="true"/>.
        /// </summary>
        /// <typeparam name="T">Type of the class that <see cref="IEnumerable{T}"/> enumerates.</typeparam>
        /// <param name="source">Instance of the <see cref="IEnumerable{T}"/> that Where-clause is applied if <paramref name="apply"/> is <see cref="true"/>.</param>
        /// <param name="apply">If <see cref="true"/>, <paramref name="predicate"/> will be applied to <paramref name="source"/>.</param>
        /// <param name="predicate">The clause that will be applied if <paramref name="apply"/> is <see cref="true"/>.</param>
        /// <returns><paramref name="source"/> with Where-clause applied if <paramref name="apply"/> is <see cref="true"/>.</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool apply, Func<T, bool> predicate)
        {
            if (apply)
                return source.Where(predicate);
            else
                return source;
        }
    }
}

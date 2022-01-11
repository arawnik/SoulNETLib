using System.Linq.Expressions;

namespace SoulNETLib.Common.Extension
{
    /// <summary>
    /// Define extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Apply the Where-clause into <paramref name="source"/> if <paramref name="apply"/> is <see cref="true"/>.
        /// </summary>
        /// <typeparam name="T">Type of the class that <see cref="IQueryable{T}"/> enumerates.</typeparam>
        /// <param name="source">Instance of the <see cref="IQueryable{T}"/> that Where-clause is applied if <paramref name="apply"/> is <see cref="true"/>.</param>
        /// <param name="apply">If <see cref="true"/>, <paramref name="predicate"/> will be applied to <paramref name="source"/>.</param>
        /// <param name="predicate">The clause that will be applied if <paramref name="apply"/> is <see cref="true"/>.</param>
        /// <returns><paramref name="source"/> with Where-clause applied if <paramref name="apply"/> is <see cref="true"/>.</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool apply, Expression<Func<T, bool>> predicate)
        {
            if (apply)
                return source.Where(predicate);
            else
                return source;
        }
    }
}

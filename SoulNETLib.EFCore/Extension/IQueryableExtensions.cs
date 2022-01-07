using SoulNETLib.EFCore.Collection;

namespace SoulNETLib.EFCore.Extension
{
    /// <summary>
    /// Define extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Paginate the results of <see cref="IQueryable{T}"/> and create <see cref="PaginatedList{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the stored items.</typeparam>
        /// <param name="source">The <see cref="IQueryable{T}"/> from which <see cref="PaginatedList{T}"/> is created from.</param>
        /// <param name="currentPage">One-based index of the presented page.</param>
        /// <param name="pageSize">The amount of rows per page.</param>
        /// <returns>Contents and related important information of a page within <paramref name="source"/>.</returns>
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int currentPage, int pageSize)
            where T : class
        {
            return await PaginatedList<T>.CreateAsync(source, currentPage, pageSize);
        }

        /// <summary>
        /// Paginate the results of <see cref="IQueryable{T}"/> and create <see cref="PaginatedResult{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the stored items.</typeparam>
        /// <param name="source">The <see cref="IQueryable{T}"/> from which <see cref="PaginatedResult{T}"/> is created from.</param>
        /// <param name="currentPage">One-based index of the presented page.</param>
        /// <param name="pageSize">The amount of rows per page.</param>
        /// <returns>Contents and related important information of a page within <paramref name="source"/>.</returns>
        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> source, int currentPage, int pageSize)
            where T : class
        {
            return await PaginatedResult<T>.CreateAsync(source, currentPage, pageSize);
        }
    }
}

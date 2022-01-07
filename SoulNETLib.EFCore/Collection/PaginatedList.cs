using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace SoulNETLib.EFCore.Collection
{
    /// <summary>
    /// Collection that stores the contents and related important information of a page within bigger dataset.
    /// </summary>
    /// <typeparam name="T">Type of the stored items.</typeparam>
    public class PaginatedList<T> : ReadOnlyCollection<T>
        where T : class
    {
        /// <summary>
        /// One-based index of the presented page.
        /// </summary>
        public readonly int CurrentPage;

        /// <summary>
        /// Size of pages. The amount of rows per page.
        /// </summary>
        public readonly int PageSize;

        /// <summary>
        /// Amount of pages in the original dataset (Not the one in the stored <see cref="Items"/> subset).
        /// </summary>
        public readonly int PageCount;

        /// <summary>
        /// Amount of rows in the original dataset (Not the one in the stored <see cref="Items"/> subset).
        /// </summary>
        public readonly long RowCount;

        /// <summary>
        /// List of <see cref="Items"/> on the presented page.
        /// </summary>
        public IList<T> PageItems => Items;

        /// <summary>
        /// Avoid using the constructor when possible, use <see cref="CreateAsync"/> instead.
        /// </summary>
        /// <param name="items"><see cref="IList{T}"/> of items on the presented page.</param>
        /// <param name="rowCount">Amount of rows in the original dataset.</param>
        /// <param name="currentPage">One-based index of the presented page.</param>
        /// <param name="pageSize">The amount of rows per page.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is <see cref="null"/>.</exception>
        public PaginatedList(IList<T> items, long rowCount, int currentPage, int pageSize)
            : base(items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            CurrentPage = currentPage;
            PageSize = pageSize;
            RowCount = rowCount;
            PageCount = (int)Math.Ceiling(rowCount / (double)PageSize);
        }

        /// <summary>
        /// <see cref="true"/> if there is previous page. <see cref="false"/> otherwise.
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        /// <summary>
        /// <see cref="true"/> if there is next page. <see cref="false"/> otherwise.
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < PageCount);
            }
        }

        /// <summary>
        /// One-based index of the first row on the presented page.
        /// </summary>
        public long FirstRowOnPage
        {
            get
            {
                return Math.Min((CurrentPage - 1) * PageSize + 1, LastRowOnPage);
            }
        }

        /// <summary>
        /// One-based index of the last row on the presented page.
        /// </summary>
        public long LastRowOnPage
        {
            get
            {
                return Math.Min(CurrentPage * PageSize, RowCount);
            }
        }

        /// <summary>
        /// Paginate the results of <see cref="IQueryable{T}"/> and create <see cref="PaginatedList{T}"/>.
        /// </summary>
        /// <param name="source">The <see cref="IQueryable{T}"/> from which <see cref="PaginatedList{T}"/> is created from.</param>
        /// <param name="currentPage">One-based index of the presented page.</param>
        /// <param name="pageSize">The amount of rows per page.</param>
        /// <returns>Contents and related important information of a page within <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see cref="null"/>.</exception>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, currentPage, pageSize);
        }
    }
}

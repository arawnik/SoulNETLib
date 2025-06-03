using System.Text.Json.Serialization;

namespace SoulNETLib.EFCore.Collection;

/// <summary>
/// Exposes <see cref="PaginatedList{T}"/> with the pagination details instead of working as a plain collection.
/// </summary>
/// <typeparam name="T">Type of the stored items.</typeparam>
/// <remarks>
/// Create the PaginatedResult out of <see cref="PaginatedList{T}"/>.
/// </remarks>
/// <param name="list">List that stores the pagination details.</param>
public class PaginatedResult<T>(PaginatedList<T> list)
    where T : class
{
    /// <summary>
    /// Readonly handle for the collection that stores the contents and related information.
    /// </summary>
    private readonly PaginatedList<T> _list = list;

    /// <summary>
    /// List of <see cref="Items"/> on the presented page.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonPropertyOrder(5)]
    public IList<T> Items => _list.PageItems;

    /// <summary>
    /// One-based index of the presented page.
    /// </summary>
    [JsonPropertyName("page")]
    [JsonPropertyOrder(1)]
    public int CurrentPage => _list.CurrentPage;

    /// <summary>
    /// Size of pages. The amount of rows per page.
    /// </summary>
    [JsonPropertyName("size")]
    [JsonPropertyOrder(2)]
    public int PageSize => _list.PageSize;

    /// <summary>
    /// Amount of pages in the original dataset (Not the one in the stored <see cref="Items"/> subset).
    /// </summary>
    [JsonPropertyName("pages")]
    [JsonPropertyOrder(3)]
    public int PageCount => _list.PageCount;

    /// <summary>
    /// Amount of rows in the original dataset (Not the one in the stored <see cref="Items"/> subset).
    /// </summary>
    [JsonPropertyName("rows")]
    [JsonPropertyOrder(4)]
    public long RowCount => _list.RowCount;

    /// <summary>
    /// Paginate the results of <see cref="IQueryable{T}"/> and create <see cref="PaginatedResult{T}"/>.
    /// </summary>
    /// <param name="source">The <see cref="IQueryable{T}"/> from which <see cref="PaginatedResult{T}"/> is created from.</param>
    /// <param name="currentPage">One-based index of the presented page.</param>
    /// <param name="pageSize">The amount of rows per page.</param>
    /// <returns>Contents and related important information of a page within <paramref name="source"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
    public static async Task<PaginatedResult<T>> CreateAsync(
        IQueryable<T> source,
        int currentPage,
        int pageSize
    )
    {
        return new PaginatedResult<T>(
            await PaginatedList<T>.CreateAsync(source, currentPage, pageSize)
        );
    }
}

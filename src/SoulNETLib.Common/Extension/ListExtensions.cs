namespace SoulNETLib.Common.Extension;

/// <summary>
/// Define extension methods for <see cref="List{T}"/> and <see cref="IList{T}"/>.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Replace entity in <paramref name="source"/> that matches <paramref name="match"/> with <paramref name="newValue"/>.
    /// </summary>
    /// <typeparam name="T">Type of the class that <see cref="List{T}"/> contains.</typeparam>
    /// <param name="source">Instance of <see cref="List{T}"/> where replace happens.</param>
    /// <param name="match">Predicate used to check for matching index.</param>
    /// <param name="newValue">New value that will be set to index.</param>
    /// <returns>Index of the replaced entity.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static int Replace<T>(this List<T> source, Predicate<T> match, T newValue)
    {
        ArgumentNullException.ThrowIfNull(source);

        var index = source.FindIndex(match);
        if (index != -1)
            source[index] = newValue;
        return index;
    }

    /// <summary>
    /// Replace all entities that match <paramref name="oldValue"/> with <paramref name="newValue"/>.
    /// </summary>
    /// <typeparam name="T">Type of the class that <see cref="List{T}"/> contains.</typeparam>
    /// <param name="source">Instance of <see cref="List{T}"/> where replace happens.</param>
    /// <param name="oldValue">Old value that will be replaced.</param>
    /// <param name="newValue">New value that will be set to indexes.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void ReplaceAll<T>(this IList<T> source, T oldValue, T newValue)
    {
        ArgumentNullException.ThrowIfNull(source);

        int index;
        do
        {
            index = source.IndexOf(oldValue);
            if (index != -1)
                source[index] = newValue;
        } while (index != -1);
    }
}

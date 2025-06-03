namespace SoulNETLib.Common.Extension;

/// <summary>
/// Define extension methods for <see cref="Exception"/>.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Get the message with as much detail as possible.
    /// </summary>
    /// <param name="source">The source <see cref="Exception"/> from which we get the message.</param>
    /// <returns>The message with as much detail as possible.</returns>
    public static string GetDetailMessage(this Exception source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.InnerException?.Message ?? source.Message;
    }
}

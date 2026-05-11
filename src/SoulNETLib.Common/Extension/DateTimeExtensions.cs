namespace SoulNETLib.Common.Extension;

/// <summary>
/// Extension methods for <see cref="DateTime"/> for UTC kind handling.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Marks the <see cref="DateTime"/> as <see cref="DateTimeKind.Utc"/> without converting the value.
    /// Use when a date-only or already-UTC value must be stored in a UTC-typed database column.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to mark as UTC.</param>
    /// <returns>A new <see cref="DateTime"/> with <see cref="DateTimeKind.Utc"/> and the same date/time value.</returns>
    public static DateTime AsUtc(this DateTime dateTime) =>
        DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

    /// <summary>
    /// Marks the <see cref="DateTime"/> as <see cref="DateTimeKind.Utc"/> without converting the value,
    /// or returns <c>null</c> if the input is <c>null</c>.
    /// </summary>
    /// <param name="dateTime">The nullable <see cref="DateTime"/> to mark as UTC.</param>
    /// <returns>A new <see cref="DateTime"/> with <see cref="DateTimeKind.Utc"/>, or <c>null</c>.</returns>
    public static DateTime? AsUtc(this DateTime? dateTime) =>
        dateTime.HasValue ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : null;

    /// <summary>
    /// Returns <c>true</c> if the <see cref="DateTime.Kind"/> is <see cref="DateTimeKind.Utc"/>.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to check.</param>
    /// <returns><c>true</c> if <see cref="DateTime.Kind"/> equals <see cref="DateTimeKind.Utc"/>; otherwise <c>false</c>.</returns>
    public static bool IsUtc(this DateTime dateTime) => dateTime.Kind == DateTimeKind.Utc;
}

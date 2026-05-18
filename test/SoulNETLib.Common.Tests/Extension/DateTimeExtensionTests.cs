using SoulNETLib.Common.Extension;

namespace SoulNETLib.Common.Tests.Extension;

public class DateTimeExtensionTests
{
    [Fact]
    public void AsUtc_LocalKind_ReturnsUtcKind()
    {
        // Arrange
        var local = new DateTime(2026, 5, 12, 10, 30, 0, DateTimeKind.Local);

        // Act
        var result = local.AsUtc();

        // Assert
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void AsUtc_UnspecifiedKind_ReturnsUtcKind()
    {
        // Arrange
        var unspecified = new DateTime(2026, 5, 12, 10, 30, 0, DateTimeKind.Unspecified);

        // Act
        var result = unspecified.AsUtc();

        // Assert
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void AsUtc_AlreadyUtc_ReturnsSameKind()
    {
        // Arrange
        var utc = new DateTime(2026, 5, 12, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var result = utc.AsUtc();

        // Assert
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void AsUtc_PreservesValue()
    {
        // Arrange
        var local = new DateTime(2026, 5, 12, 10, 30, 45, 123, DateTimeKind.Local);

        // Act
        var result = local.AsUtc();

        // Assert
        Assert.Equal(local.Ticks, result.Ticks);
    }

    [Fact]
    public void AsUtc_Nullable_WithValue_ReturnsUtcKind()
    {
        // Arrange
        DateTime? local = new DateTime(2026, 5, 12, 10, 30, 0, DateTimeKind.Local);

        // Act
        var result = local.AsUtc();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(DateTimeKind.Utc, result.Value.Kind);
        Assert.Equal(local.Value.Ticks, result.Value.Ticks);
    }

    [Fact]
    public void AsUtc_Nullable_Null_ReturnsNull()
    {
        // Arrange
        DateTime? nullDate = null;

        // Act
        var result = nullDate.AsUtc();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void IsUtc_UtcKind_ReturnsTrue()
    {
        // Arrange
        var utc = new DateTime(2026, 5, 12, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var result = utc.IsUtc();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUtc_LocalKind_ReturnsFalse()
    {
        // Arrange
        var local = new DateTime(2026, 5, 12, 10, 30, 0, DateTimeKind.Local);

        // Act
        var result = local.IsUtc();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsUtc_UnspecifiedKind_ReturnsFalse()
    {
        // Arrange
        var unspecified = new DateTime(2026, 5, 12, 10, 30, 0, DateTimeKind.Unspecified);

        // Act
        var result = unspecified.IsUtc();

        // Assert
        Assert.False(result);
    }
}

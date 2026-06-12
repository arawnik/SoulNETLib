using Microsoft.Extensions.Configuration;
using SoulNETLib.Clean.Infrastructure.Security;

namespace SoulNETLib.Clean.Infrastructure.Tests.Security;

public sealed class ConfigurationExtensionsTests : IDisposable
{
    private readonly string _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    public ConfigurationExtensionsTests() => Directory.CreateDirectory(_tempDir);

    public void Dispose()
    {
        Directory.Delete(_tempDir, recursive: true);
        GC.SuppressFinalize(this);
    }

    private static IConfiguration BuildConfig() =>
        new ConfigurationBuilder().AddInMemoryCollection().Build();

    [Fact]
    public void AddDockerSecrets_FileExists_SetsConfigurationValue()
    {
        // Arrange
        var secretFile = "db-conn";
        var secretValue = "Host=localhost;Database=mydb";
        File.WriteAllText(Path.Combine(_tempDir, secretFile), secretValue);

        var config = BuildConfig();
        var mapping = new Dictionary<string, string>
        {
            { "ConnectionStrings:Default", secretFile },
        };

        // Act
        config.AddDockerSecretsFrom(_tempDir, mapping);

        // Assert
        Assert.Equal(secretValue, config["ConnectionStrings:Default"]);
    }

    [Fact]
    public void AddDockerSecrets_FileExistsWithWhitespace_TrimsValue()
    {
        // Arrange
        var secretFile = "db-conn";
        File.WriteAllText(Path.Combine(_tempDir, secretFile), "  secret-value  \n");

        var config = BuildConfig();
        var mapping = new Dictionary<string, string>
        {
            { "ConnectionStrings:Default", secretFile },
        };

        // Act
        config.AddDockerSecretsFrom(_tempDir, mapping);

        // Assert
        Assert.Equal("secret-value", config["ConnectionStrings:Default"]);
    }

    [Fact]
    public void AddDockerSecrets_FileMissing_SkipsSilently()
    {
        // Arrange
        var config = BuildConfig();
        var mapping = new Dictionary<string, string>
        {
            { "ConnectionStrings:Default", "nonexistent-secret" },
        };

        // Act – no exception expected
        config.AddDockerSecretsFrom(_tempDir, mapping);

        // Assert
        Assert.Null(config["ConnectionStrings:Default"]);
    }

    [Fact]
    public void AddDockerSecrets_MultipleEntries_SetsAllPresentValues()
    {
        // Arrange
        File.WriteAllText(Path.Combine(_tempDir, "conn1"), "value1");
        // conn2 intentionally missing
        File.WriteAllText(Path.Combine(_tempDir, "conn3"), "value3");

        var config = BuildConfig();
        var mapping = new Dictionary<string, string>
        {
            { "Key:One", "conn1" },
            { "Key:Two", "conn2" },
            { "Key:Three", "conn3" },
        };

        // Act
        config.AddDockerSecretsFrom(_tempDir, mapping);

        // Assert
        Assert.Equal("value1", config["Key:One"]);
        Assert.Null(config["Key:Two"]);
        Assert.Equal("value3", config["Key:Three"]);
    }

    [Fact]
    public void AddDockerSecrets_EmptyMapping_DoesNothing()
    {
        // Arrange
        var config = BuildConfig();

        // Act / Assert – no exception
        config.AddDockerSecretsFrom(_tempDir, new Dictionary<string, string>());
    }
}

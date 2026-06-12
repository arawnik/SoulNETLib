using SoulNETLib.Clean.Infrastructure.Security;

namespace SoulNETLib.Clean.Infrastructure.Tests.Security;

public sealed class DockerSecretsTests : IDisposable
{
    private readonly string _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    // Use a unique prefix for env vars to avoid collisions with real environment variables.
    private const string EnvVarPrefix = "SOULNETLIB_TEST_";

    public DockerSecretsTests() => Directory.CreateDirectory(_tempDir);

    public void Dispose()
    {
        Directory.Delete(_tempDir, recursive: true);

        // Clean up any env vars set during tests.
        Environment.SetEnvironmentVariable($"{EnvVarPrefix}KEY1", null);
        Environment.SetEnvironmentVariable($"{EnvVarPrefix}KEY2", null);
        Environment.SetEnvironmentVariable($"{EnvVarPrefix}KEY3", null);

        GC.SuppressFinalize(this);
    }

    [Fact]
    public void LoadAsEnvironmentVariables_FileExists_SetsEnvironmentVariable()
    {
        // Arrange
        var secretFile = "my-secret";
        var secretValue = "super-secret-value";
        File.WriteAllText(Path.Combine(_tempDir, secretFile), secretValue);

        var mapping = new Dictionary<string, string> { { $"{EnvVarPrefix}KEY1", secretFile } };

        // Act
        DockerSecrets.LoadAsEnvironmentVariablesFrom(_tempDir, mapping);

        // Assert
        Assert.Equal(secretValue, Environment.GetEnvironmentVariable($"{EnvVarPrefix}KEY1"));
    }

    [Fact]
    public void LoadAsEnvironmentVariables_FileExistsWithWhitespace_TrimsValue()
    {
        // Arrange
        var secretFile = "my-secret";
        File.WriteAllText(Path.Combine(_tempDir, secretFile), "  trimmed-value  \n");

        var mapping = new Dictionary<string, string> { { $"{EnvVarPrefix}KEY1", secretFile } };

        // Act
        DockerSecrets.LoadAsEnvironmentVariablesFrom(_tempDir, mapping);

        // Assert
        Assert.Equal("trimmed-value", Environment.GetEnvironmentVariable($"{EnvVarPrefix}KEY1"));
    }

    [Fact]
    public void LoadAsEnvironmentVariables_FileMissing_SkipsSilently()
    {
        // Arrange
        var mapping = new Dictionary<string, string>
        {
            { $"{EnvVarPrefix}KEY1", "nonexistent-secret" },
        };

        // Act – no exception expected
        DockerSecrets.LoadAsEnvironmentVariablesFrom(_tempDir, mapping);

        // Assert
        Assert.Null(Environment.GetEnvironmentVariable($"{EnvVarPrefix}KEY1"));
    }

    [Fact]
    public void LoadAsEnvironmentVariables_MultipleEntries_SetsAllPresentValues()
    {
        // Arrange
        File.WriteAllText(Path.Combine(_tempDir, "secret1"), "val1");
        // secret2 intentionally missing
        File.WriteAllText(Path.Combine(_tempDir, "secret3"), "val3");

        var mapping = new Dictionary<string, string>
        {
            { $"{EnvVarPrefix}KEY1", "secret1" },
            { $"{EnvVarPrefix}KEY2", "secret2" },
            { $"{EnvVarPrefix}KEY3", "secret3" },
        };

        // Act
        DockerSecrets.LoadAsEnvironmentVariablesFrom(_tempDir, mapping);

        // Assert
        Assert.Equal("val1", Environment.GetEnvironmentVariable($"{EnvVarPrefix}KEY1"));
        Assert.Null(Environment.GetEnvironmentVariable($"{EnvVarPrefix}KEY2"));
        Assert.Equal("val3", Environment.GetEnvironmentVariable($"{EnvVarPrefix}KEY3"));
    }

    [Fact]
    public void LoadAsEnvironmentVariables_EmptyMapping_DoesNothing()
    {
        // Act / Assert – no exception
        DockerSecrets.LoadAsEnvironmentVariablesFrom(_tempDir, new Dictionary<string, string>());
    }
}

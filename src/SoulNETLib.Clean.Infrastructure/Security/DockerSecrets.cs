namespace SoulNETLib.Clean.Infrastructure.Security;

/// <summary>
/// Utility class for loading Docker secrets from the standard <c>/run/secrets</c> mount
/// into environment variables.
/// </summary>
public static class DockerSecrets
{
    internal const string SecretDir = "/run/secrets";

    /// <summary>
    /// For each mapping of environment variable name → secret file name, reads the contents
    /// of <c>/run/secrets/{secretFileName}</c> (if it exists) and sets the corresponding
    /// environment variable via <see cref="System.Environment.SetEnvironmentVariable(string, string?)"/>.
    /// Files that do not exist are silently skipped (e.g., during local development).
    /// </summary>
    /// <param name="envVarToSecretFileName">
    /// A mapping from environment variable names to Docker secret file names.
    /// </param>
    public static void LoadAsEnvironmentVariables(
        IDictionary<string, string> envVarToSecretFileName
    )
    {
        ArgumentNullException.ThrowIfNull(envVarToSecretFileName);
        LoadAsEnvironmentVariablesFrom(SecretDir, envVarToSecretFileName);
    }

    /// <summary>
    /// Internal overload that accepts a custom secrets directory; used by unit tests to
    /// avoid requiring an actual <c>/run/secrets</c> mount.
    /// </summary>
    internal static void LoadAsEnvironmentVariablesFrom(
        string secretDir,
        IDictionary<string, string> envVarToSecretFileName
    )
    {
        foreach (var kv in envVarToSecretFileName)
        {
            var path = Path.Combine(secretDir, kv.Value);
            if (File.Exists(path))
                Environment.SetEnvironmentVariable(kv.Key, File.ReadAllText(path).Trim());
        }
    }
}

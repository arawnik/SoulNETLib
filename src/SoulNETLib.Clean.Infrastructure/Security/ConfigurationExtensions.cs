using Microsoft.Extensions.Configuration;

namespace SoulNETLib.Clean.Infrastructure.Security;

/// <summary>
/// Extension methods for loading Docker secrets into <see cref="IConfiguration"/>.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Loads each Docker secret file under <c>/run/secrets</c> into the configuration
    /// using the provided key-to-filename mapping.
    /// Files that do not exist are silently skipped (e.g., during local development).
    /// </summary>
    /// <remarks>
    /// Call this before building the service container so that secrets are available
    /// to any configuration-dependent services (e.g., connection strings).
    /// </remarks>
    /// <param name="configuration">The configuration instance to write secret values into.</param>
    /// <param name="configKeyToSecretFileName">
    /// A mapping from colon-delimited configuration keys (e.g. <c>"ConnectionStrings:Default"</c>)
    /// to Docker secret file names (e.g. <c>"my-db-conn"</c>).
    /// </param>
    public static void AddDockerSecrets(
        this IConfiguration configuration,
        IDictionary<string, string> configKeyToSecretFileName
    )
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(configKeyToSecretFileName);
        AddDockerSecretsFrom(configuration, DockerSecrets.SecretDir, configKeyToSecretFileName);
    }

    /// <summary>
    /// Internal overload that accepts a custom secrets directory; used by unit tests to
    /// avoid requiring an actual <c>/run/secrets</c> mount.
    /// </summary>
    internal static void AddDockerSecretsFrom(
        this IConfiguration configuration,
        string secretDir,
        IDictionary<string, string> configKeyToSecretFileName
    )
    {
        foreach (var kv in configKeyToSecretFileName)
        {
            var path = Path.Combine(secretDir, kv.Value);
            // Directly override the IConfiguration value if exists.
            if (File.Exists(path))
                configuration[kv.Key] = File.ReadAllText(path).Trim();
        }
    }
}

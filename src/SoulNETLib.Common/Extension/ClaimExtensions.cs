using System.Security.Claims;
using System.Security.Principal;

namespace SoulNETLib.Common.Extension;

/// <summary>
/// Define extension methods related to <see cref="Claim"/>.
/// </summary>
public static class ClaimExtensions
{
    /// <summary>
    /// Get the user id from <see paramref="source"/>. Assumes that Id is stored to <see cref="ClaimTypes.NameIdentifier"/>.
    /// </summary>
    /// <param name="source">The source <see cref="IPrincipal"/>.</param>
    /// <returns>The user id from <paramref name="source"/>.</returns>
    /// <exception cref="NullReferenceException">If unable to find UserId from <paramref name="source"/>.</exception>
    public static string GetUserId(this IPrincipal source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var claimsIdentity = (ClaimsIdentity?)source.Identity;
#pragma warning disable CA2201 // Do not raise reserved exception types
        var claim =
            claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new NullReferenceException(
                "Unable to get user id from principal" + source.Identity == null
                    ? "From unknown ClaimsIdentity"
                    : $"from {source.Identity?.Name}"
            );
#pragma warning restore CA2201 // Do not raise reserved exception types
        return claim.Value;
    }
}

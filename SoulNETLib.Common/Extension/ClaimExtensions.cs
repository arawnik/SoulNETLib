using System.Security.Claims;
using System.Security.Principal;

namespace SoulNETLib.Common.Extension
{
    /// <summary>
    /// Define extension methods related to <see cref="Claim"/>.
    /// </summary>
    public static class ClaimExtensions
    {
        /// <summary>
        /// Get the user id from <see cref="source"/>. Assumes that Id is stored to <see cref="ClaimTypes.NameIdentifier"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IPrincipal"/>.</param>
        /// <returns>The user id from <paramref name="source"/>.</returns>
        /// <exception cref="NullReferenceException">If unable to find UserId from <paramref name="source"/>.</exception>
        public static string GetUserId(this IPrincipal source)
        {
            var claimsIdentity = (ClaimsIdentity?)source.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier) ?? throw new NullReferenceException(
                "Unable to get user id from principal" + source.Identity == null ? "From unknown ClaimsIdentity" : $"from {source.Identity?.Name}" );
            return claim.Value;
        }
    }
}

using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Parivar.Utility.Extension
{
    public static class ClaimsExtensions
    {
        public static string GetClaimValue(this ClaimsPrincipal principal, string key)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            return principal.Claims.FirstOrDefault(x => x.Type == key)?.Value ?? "";
        }

        public static string GetClaimValue(this IIdentity identity, string key)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;

            Claim claim = claimsIdentity.FindFirst(key);

            return claim.Value;
        }

        public static void UpdateClaim(this ClaimsIdentity identity, string key, string value)
        {
            if (identity == null)
                return;

            // check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);

            // add new claim
            identity.AddClaim(new Claim(key, value));

        }
    }
}

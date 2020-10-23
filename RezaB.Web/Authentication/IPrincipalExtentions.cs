using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Authentication
{
    public static class IPrincipalExtentions
    {
        public static string GiveUsername(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            return (claim == null) ? null : claim.Value;
        }

        /// <summary>
        /// Gives user id.
        /// </summary>
        /// <param name="User">The user.</param>
        /// <returns></returns>
        public static long? GiveUserId(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            return (claim == null) ? (long?)null : long.Parse(claim.Value);
        }

        /// <summary>
        /// Gets user permissions.
        /// </summary>
        /// <param name="User">The user.</param>
        /// <returns></returns>
        public static string[] GetPermissions(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var permissionClaims = identity.Claims.Where(c => c.Type == "permission");
            return permissionClaims.Select(c => c.Value).ToArray();
        }

        /// <summary>
        /// Checks if user has a specific permission.
        /// </summary>
        /// <param name="User">The user.</param>
        /// <param name="permission">Specified permission.</param>
        /// <returns></returns>
        public static bool HasPermission(this IPrincipal User, string permission)
        {
            return User.GetPermissions().Contains(permission.ToLower(System.Globalization.CultureInfo.InvariantCulture).Trim());
        }
    }
}

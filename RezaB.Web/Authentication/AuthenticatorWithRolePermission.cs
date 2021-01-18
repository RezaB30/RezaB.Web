using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Authentication
{
    public class Authenticator<TDB, TUT, TRT, TPT, THA> : Authenticator<TDB, TUT, THA> where TDB : DbContext, new() where TUT : class where TRT : class where TPT : class where THA : HashAlgorithm
    {
        #region protected properties
        protected string RoleNameColumn { get; set; }

        protected string PermissionNameColumn { get; set; }
        #endregion
        /// <summary>
        /// Creates a new authenticator instance.
        /// </summary>
        /// <param name="userIDColumn">The expression for user's id column.</param>
        /// <param name="userDisplayNameColumn">The expression for user's display name column.</param>
        /// <param name="usernameColumn">The expression for user's username column.</param>
        /// <param name="userPasswordColumn">The expression for user's password column.</param>
        /// <param name="userEnabledColumn">The expression for user's enabled column.</param>
        /// <param name="roleNameColumn">The expression for role's name column.</param>
        /// <param name="permissionNameColumn">The expression for permission's name column.</param>
        public Authenticator(
            Expression<Func<TUT, object>> userIDColumn,
            Expression<Func<TUT, string>> userDisplayNameColumn,
            Expression<Func<TUT, string>> usernameColumn,
            Expression<Func<TUT, string>> userPasswordColumn,
            Expression<Func<TUT, bool>> userEnabledColumn,
            Expression<Func<TRT, string>> roleNameColumn,
            Expression<Func<TPT, string>> permissionNameColumn
            ) : base(userIDColumn, userDisplayNameColumn, usernameColumn, userPasswordColumn, userEnabledColumn)
        {
            RoleNameColumn = GetMemberName(roleNameColumn.Body);
            PermissionNameColumn = GetMemberName(permissionNameColumn.Body);
        }
        /// <summary>
        /// Signs in user with id without password check.
        /// </summary>
        /// <param name="owinContext">Owin context.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="extraClaims">Extra claims to be added.</param>
        /// <returns></returns>
        public override bool SignIn(IOwinContext owinContext, int userId, IEnumerable<Claim> extraClaims = null)
        {
            using (TDB db = new TDB())
            {
                var user = GetUser(db, userId);
                if (user == null)
                    return false;
                // extract properties
                var isEnabled = user.GetType().GetProperty(UserEnabledColumn).GetValue(user) as bool?;
                // enabled
                if (isEnabled != true)
                    return false;
                var name = user.GetType().GetProperty(UserDisplayNameColumn).GetValue(user) as string;
                var email = user.GetType().GetProperty(UsernameColumn).GetValue(user) as string;
                var role = user.GetType().GetProperty(typeof(TRT).Name).GetValue(user);
                var roleName = typeof(TRT).GetProperty(RoleNameColumn).GetValue(role) as string;
                var permissions = role.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.GenericTypeArguments.Contains(typeof(TPT))).GetValue(role) as IEnumerable<TPT>;
                // add claims
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                };
                // extra claims
                if (extraClaims != null)
                    claims.AddRange(extraClaims);
                var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                if (role == null)
                {
                    return false;
                }
                identity.AddClaim(new Claim(ClaimTypes.Role, roleName.Trim().ToLower()));
                foreach (var permission in permissions)
                {
                    var permissionName = typeof(TPT).GetProperty(PermissionNameColumn).GetValue(permission) as string;

                    identity.AddClaim(new Claim("permission", permissionName.ToLower(System.Globalization.CultureInfo.InvariantCulture).Trim()));
                }
                // sign in
                var authManager = owinContext.Authentication;
                authManager.SignIn(identity);
                return true;
            }
        }
    }
}

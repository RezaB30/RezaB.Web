using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Claims;
using Microsoft.Owin;
using System.Security.Cryptography;
using System.Security.Principal;

namespace RezaB.Web.Authentication
{
    /// <summary>
    /// Utility class for authenticating user using owin context.
    /// </summary>
    /// <typeparam name="TDB">Data base context.</typeparam>
    /// <typeparam name="TUT">User table entity.</typeparam>
    /// <typeparam name="THA">Hashing algorithm</typeparam>
    public class Authenticator<TDB, TUT, THA> where TDB : DbContext, new() where TUT : class where THA : HashAlgorithm
    {
        /// <summary>
        /// Signs in a user with user id without checking for password.
        /// </summary>
        /// <param name="owinContext">Owin context.</param>
        /// <param name="userId">User id.</param>
        /// <param name="extraClaims">Extra claims to add. (Name, Email and NameIdentifier is added automatically)</param>
        /// <returns></returns>
        public bool SignIn(IOwinContext owinContext, int userId, IEnumerable<Claim> extraClaims = null)
        {
            using (TDB db = new TDB())
            {
                var user = GetUser(db, userId);
                if (user == null)
                    return false;
                // extract properties
                var name = user.GetType().GetProperty("Name").GetValue(user) as string;
                var email = user.GetType().GetProperty("Username").GetValue(user) as string;
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
                // sign in
                var authManager = owinContext.Authentication;
                authManager.SignIn(identity);
                return true;
            }
        }
        /// <summary>
        /// Signs in user with user id without checking for password and adds permissions and roles.
        /// </summary>
        /// <typeparam name="TRT">Role table entity.</typeparam>
        /// <typeparam name="TPT">Permission table entity.</typeparam>
        /// <param name="owinContext"></param>
        /// <param name="userId"></param>
        /// <param name="extraClaims">Extra claims to add. (Name, Email, NameIdentifier, Role and Permissions is added automatically)</param>
        /// <returns></returns>
        public bool SignIn<TRT, TPT>(IOwinContext owinContext, int userId, IEnumerable<Claim> extraClaims = null)
        {
            using (TDB db = new TDB())
            {
                var user = GetUser(db, userId);
                if (user == null)
                    return false;
                // extract properties
                var isEnabled = user.GetType().GetProperty("IsEnabled").GetValue(user) as bool?;
                // enabled
                if (isEnabled != true)
                    return false;
                var name = user.GetType().GetProperty("Name").GetValue(user) as string;
                var email = user.GetType().GetProperty("Email").GetValue(user) as string;
                var role = user.GetType().GetProperty(typeof(TRT).Name).GetValue(user);
                var roleName = typeof(TRT).GetProperty("Name").GetValue(role) as string;
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
                    var permissionName = typeof(TPT).GetProperty("Name").GetValue(permission) as string;

                    identity.AddClaim(new Claim("permission", permissionName.ToLower(System.Globalization.CultureInfo.InvariantCulture).Trim()));
                }
                // sign in
                var authManager = owinContext.Authentication;
                authManager.SignIn(identity);
                return true;
            }
        }
        /// <summary>
        /// Signs in a user with username and checks the password.
        /// </summary>
        /// <param name="owinContext">Owin context.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns></returns>
        public bool SignIn(IOwinContext owinContext, string username, string password)
        {
            var userId = Authenticate(username, password);
            if (!userId.HasValue)
                return false;
            return SignIn(owinContext, userId.Value);
        }

        public int? Authenticate(string username, string password)
        {
            using (TDB db = new TDB())
            {
                var user = GetUser(db, username);
                if (user == null)
                    return null;
                var passwordInfo = user.GetType().GetProperty("Password");
                var passwordValue = passwordInfo.GetValue(user) as string;
                var IdInfo = user.GetType().GetProperty("ID");
                var IdValue = IdInfo.GetValue(user) as int?;
                if (passwordValue.ToLower() == GetPasswordHash(password))
                    return IdValue;
                return null;
            }
        }
        /// <summary>
        /// Converts plain text password to hashed value.
        /// </summary>
        /// <param name="plainPassword">Plain password.</param>
        /// <returns></returns>
        public string GetPasswordHash(string plainPassword)
        {
            THA algorithm = (THA)HashAlgorithm.Create(typeof(THA).Name);
            var bytes = Encoding.UTF8.GetBytes(plainPassword);
            var hashed = algorithm.ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashed.Length; i++)
            {
                builder.Append(hashed[i].ToString("X2"));
            }

            return builder.ToString().ToLower();
        }
        /// <summary>
        /// Signs out user.
        /// </summary>
        /// <param name="owinContext">Owin context.</param>
        public void SignOut(IOwinContext owinContext)
        {
            var authManager = owinContext.Authentication;
            authManager.SignOut();
        }

        #region private methods

        private TUT GetUser(TDB db, string username)
        {
            var parameterExp = Expression.Parameter(typeof(TUT), "u");
            var memberExp = Expression.Property(parameterExp, "Email");
            var lower = Expression.Call(memberExp, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
            var constantExp = Expression.Constant(username.ToLower(), typeof(string));
            var body = Expression.Equal(lower, constantExp);
            var userEnabled = Expression.Property(parameterExp, "IsEnabled");
            body = Expression.AndAlso(body, userEnabled);
            var finalExp = Expression.Lambda<Func<TUT, bool>>(body, new[] { parameterExp });
            var user = db.Set<TUT>().Where(finalExp).FirstOrDefault();
            return user;
        }

        private TUT GetUser(TDB db, int id)
        {
            var user = db.Set<TUT>().Find(id);
            var IsEnabled = (bool)user.GetType().GetProperty("IsEnabled").GetValue(user);
            if (IsEnabled)
                return user;
            return null;
        }

        #endregion
    }
}

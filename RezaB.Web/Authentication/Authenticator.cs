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
        #region protected properties
        protected string UserIDColumn { get; set; }

        protected string UserDisplayNameColumn { get; set; }

        protected string UsernameColumn { get; set; }

        protected string UserEnabledColumn { get; set; }

        protected string UserPasswordColumn { get; set; }
        #endregion
        /// <summary>
        /// Creates a new authenticator instance.
        /// </summary>
        /// <param name="userIDColumn">The expression for user's id column.</param>
        /// <param name="userDisplayNameColumn">The expression for user's display name column.</param>
        /// <param name="usernameColumn">The expression for user's username column.</param>
        /// <param name="userPasswordColumn">The expression for user's password column.</param>
        /// <param name="userEnabledColumn">The expression for user's enabled column.</param>
        public Authenticator(
            Expression<Func<TUT, object>> userIDColumn,
            Expression<Func<TUT, string>> userDisplayNameColumn,
            Expression<Func<TUT, string>> usernameColumn,
            Expression<Func<TUT, string>> userPasswordColumn,
            Expression<Func<TUT, bool>> userEnabledColumn
            )
        {
            UserIDColumn = GetMemberName(userIDColumn.Body);
            UserDisplayNameColumn = GetMemberName(userDisplayNameColumn.Body);
            UsernameColumn = GetMemberName(usernameColumn.Body);
            UserEnabledColumn = GetMemberName(userEnabledColumn.Body);
            UserPasswordColumn = GetMemberName(userPasswordColumn.Body);
        }

        /// <summary>
        /// Signs in a user with username and checks the password.
        /// </summary>
        /// <param name="owinContext">Owin context.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="extraClaims">Extra claims to be added.</param>
        /// <returns></returns>
        public bool SignIn(IOwinContext owinContext, string username, string password, IEnumerable<Claim> extraClaims = null)
        {
            var userId = Authenticate(username, password);
            if (!userId.HasValue)
                return false;
            return SignIn(owinContext, userId.Value, extraClaims);
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
                builder.Append(hashed[i].ToString("x2"));
            }

            return builder.ToString();
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
        /// <summary>
        /// Signs in user with id without password check.
        /// </summary>
        /// <param name="owinContext">Owin context.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="extraClaims">Extra claims to be added.</param>
        /// <returns></returns>
        public virtual bool SignIn(IOwinContext owinContext, int userId, IEnumerable<Claim> extraClaims = null)
        {
            using (TDB db = new TDB())
            {
                var user = GetUser(db, userId);
                if (user == null)
                    return false;
                // extract properties
                var name = user.GetType().GetProperty(UserDisplayNameColumn).GetValue(user) as string;
                var email = user.GetType().GetProperty(UsernameColumn).GetValue(user) as string;
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
        /// Authenticates user with username and password, if authenticated returns user id.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns></returns>
        public int? Authenticate(string username, string password)
        {
            using (TDB db = new TDB())
            {
                var user = GetUser(db, username);
                if (user == null)
                    return null;
                var passwordInfo = user.GetType().GetProperty(UserPasswordColumn);
                var passwordValue = passwordInfo.GetValue(user) as string;
                var IdInfo = user.GetType().GetProperty(UserIDColumn);
                var IdValue = IdInfo.GetValue(user) as int?;
                if (passwordValue.ToLower() == GetPasswordHash(password))
                    return IdValue;
                return null;
            }
        }
        #region protected methods
        protected TUT GetUser(TDB db, string username)
        {
            var parameterExp = Expression.Parameter(typeof(TUT), "u");
            var memberExp = Expression.Property(parameterExp, UsernameColumn);
            var lower = Expression.Call(memberExp, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
            var constantExp = Expression.Constant(username.ToLower(), typeof(string));
            var body = Expression.Equal(lower, constantExp);
            var userEnabled = Expression.Property(parameterExp, UserEnabledColumn);
            body = Expression.AndAlso(body, userEnabled);
            var finalExp = Expression.Lambda<Func<TUT, bool>>(body, new[] { parameterExp });
            var user = db.Set<TUT>().Where(finalExp).FirstOrDefault();
            return user;
        }

        protected TUT GetUser(TDB db, int id)
        {
            var user = db.Set<TUT>().Find(id);
            var IsEnabled = (bool)user.GetType().GetProperty(UserEnabledColumn).GetValue(user);
            if (IsEnabled)
                return user;
            return null;
        }
      
        protected string GetMemberName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member.Name;
                case ExpressionType.Convert:
                    return GetMemberName(((UnaryExpression)expression).Operand);
                default:
                    throw new NotSupportedException(expression.NodeType.ToString());
            }
        }
        #endregion
    }
}

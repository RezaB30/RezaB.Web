using RezaB.Web.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RezaB.Web.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizePermissionAttribute : AuthorizeAttribute
    {
        public string Permissions { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var permissions = Permissions == null ? Enumerable.Empty<string>() : Permissions.Split(',').Select(p => p.Trim().ToLower(System.Globalization.CultureInfo.InvariantCulture));
            var userPermissions = httpContext.User.GetPermissions();

            foreach (var permission in permissions)
            {
                if (userPermissions.Contains(permission))
                {
                    return true;
                }
            }

            var roles = Roles.Split(',').Select(p => p.Trim().ToLower());

            foreach (var role in roles)
            {
                if (httpContext.User.IsInRole(role))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

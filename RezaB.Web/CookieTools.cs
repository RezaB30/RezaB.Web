using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RezaB.Web
{
    public static class CookieTools
    {
        public static void SetCultureInfo(HttpCookieCollection allCookies, string value)
        {
            var cookie = new HttpCookie("culture", value);
            cookie.Expires = DateTime.Now.AddYears(5);
            allCookies.Set(cookie);
        }

        public static string getCulture(HttpCookieCollection allCookies)
        {
            var cookie = allCookies.Get("culture");
            var defaultValue = "tr-tr";

            if (cookie == null)
                return defaultValue;

            return cookie.Value;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RezaB.Web
{
    /// <summary>
    /// Contains utility methods for urls.
    /// </summary>
    public static class UrlUtilities
    {
        /// <summary>
        /// Adds or modifies a parameter in a url query string.
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="newValue">The new value for the parameter</param>
        /// <param name="uriBuilder">The uri builder to change the query string.</param>
        public static void AddOrModifyQueryStringParameter(string parameterName, string newValue, UriBuilder uriBuilder)
        {
            Regex parameter = new Regex(@"([\?|&]" + parameterName + "=.[^&^#]*)", RegexOptions.ECMAScript);
            bool foundMatch = false;
            var queryString = uriBuilder.Query;

            queryString = parameter.Replace(queryString, m =>
            {
                var match = m.Value;
                var first = match.FirstOrDefault();
                var last = match.LastOrDefault();
                string lasttoAdd = (last == '&') ? "&" : (last == '#') ? "#" : null;
                foundMatch = true;
                return first + parameterName + "=" + newValue + lasttoAdd;
            });
            if (!foundMatch)
            {
                if (string.IsNullOrEmpty(queryString) || queryString == "?")
                {
                    queryString = parameterName + "=" + newValue;
                }
                else {
                    queryString += "&" + parameterName + "=" + newValue;
                }
            }
            queryString = queryString.Replace("?", "");
            uriBuilder.Query = queryString;
        }

        public static void RemoveQueryStringParameter(string parameterName, UriBuilder uriBuilder)
        {
            Regex parameter = new Regex(@"([\?|&]" + parameterName + @"=.[[&]?|$])", RegexOptions.ECMAScript);
            var queryString = uriBuilder.Query;

            queryString = parameter.Replace(queryString, m =>
            {
                var match = m.Value;
                var first = match.FirstOrDefault();
                var last = match.LastOrDefault();
                string lasttoAdd = (last == '&') ? "&" : null;
                return lasttoAdd;
            });

            uriBuilder.Query = queryString.Replace("?", "");
        }
    }
}

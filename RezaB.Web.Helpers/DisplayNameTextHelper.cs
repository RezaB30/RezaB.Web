using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RezaB.Web.Helpers
{
    public static class DisplayNameTextHelper
    {
        public static string DisplayNameTextFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, object htmlAttributes = null)
        {
            return HttpUtility.HtmlDecode(helper.DisplayNameFor(expression).ToString());
        }
    }
}

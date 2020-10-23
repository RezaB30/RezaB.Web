using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RezaB.Web.Helpers
{
    public static class PartialForHelper
    {
        public static MvcHtmlString PartialFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, string viewName)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(fieldName);
            var model = metadata.Model;

            if (viewName == null)
            {
                viewName = metadata.TemplateHint == null
                    ? typeof(TResult).Name    // Class name
                    : metadata.TemplateHint;    // UIHint("template name")
            }

            return helper.Partial(viewName, model, new ViewDataDictionary(helper.ViewData)
            {
                TemplateInfo = new TemplateInfo() { HtmlFieldPrefix = fullName }
            });
        }
    }
}

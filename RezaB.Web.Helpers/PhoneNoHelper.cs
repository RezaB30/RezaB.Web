using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RezaB.Web.Helpers
{
    public static class PhoneNoHelper
    {
        public static MvcHtmlString PhoneNoFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var value = metadata.Model as string;

            TagBuilder input = new TagBuilder("input");
            input.MergeAttribute("type", "text");
            input.MergeAttribute("maxlength", "17");
            input.MergeAttribute("placeholder", "+00 000 000 0000");
            input.AddCssClass("input-phone");
            input.GenerateId(fullName);
            input.MergeAttribute("name", fullName);
            input.MergeAttribute("value", value);
            //input.MergeAttribute("autocomplete", "off");
            if (htmlAttributes != null)
                input.MergeAdditionalAttributes(htmlAttributes);

            return new MvcHtmlString(input.ToString(TagRenderMode.SelfClosing));
        }
    }
}

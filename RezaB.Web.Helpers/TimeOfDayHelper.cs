using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RezaB.Web.Helpers
{
    public static class TimeOfDayHelper
    {
        public static MvcHtmlString TimeOfDayEditorFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, object htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var model = metadata.Model as string;

            TagBuilder input = new TagBuilder("input");
            input.MergeAttribute("type", "text");
            input.AddCssClass("time-of-day-input");
            input.MergeAttribute("maxlength", "5");
            input.MergeAttribute("placeholder", "23:59");
            input.GenerateId(fullName);
            input.MergeAttribute("name", fullName);

            input.MergeAttribute("value", model ?? string.Empty);

            if (htmlAttributes != null)
            {
                foreach (var propertyName in htmlAttributes.GetType().GetProperties().Select(property => property.Name))
                {
                    input.MergeAttribute(propertyName, htmlAttributes.GetType().GetProperty(propertyName).GetValue(htmlAttributes).ToString());
                }
            }

            return new MvcHtmlString(input.ToString(TagRenderMode.SelfClosing));
        }
    }
}

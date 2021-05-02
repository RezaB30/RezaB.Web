using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Threading;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using RezaB.Web.Helpers.DataTypes;

namespace RezaB.Web.Helpers
{
    public static class DateTimeHelper
    {
        public static MvcHtmlString ShortDateEditor(this HtmlHelper helper, string propName, DateTime? selectedDate = null, object htmlAttributes = null)
        {
            var hasValue = selectedDate != null && selectedDate.Value.Ticks > 0;
            var stringValue = hasValue ? selectedDate.Value.ToShortDateString() : string.Empty;
            var attributes = htmlAttributes != null ? htmlAttributes.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(htmlAttributes)) : new Dictionary<string, object>();
            attributes["placeholder"] = DateTime.Now.ToShortDateString();
            attributes["autocomplete"] = "off";
            return helper.TextBox(propName, stringValue, attributes);
        }

        public static MvcHtmlString ShortDateEditorFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var value = metadata.Model as DateTime?;

            return ShortDateEditor(helper, fieldName, value, htmlAttributes);
        }

        public static MvcHtmlString LongDateEditor(this HtmlHelper helper, string propName, DateWithTime selectedDate = null, object htmlAttributes = null)
        {
            var hasValue = selectedDate?.InternalValue != null && selectedDate.InternalValue?.Ticks > 0;
            var stringValue = hasValue ? selectedDate.InternalValue?.ToString("MM/dd/yyyy HH:mm") : string.Empty;
            var attributes = htmlAttributes != null ? htmlAttributes.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(htmlAttributes)) : new Dictionary<string, object>();
            attributes["placeholder"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            attributes["autocomplete"] = "off";
            return helper.TextBox(propName, stringValue, attributes);
        }

        public static MvcHtmlString LongDateEditorFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var value = metadata.Model as DateWithTime;

            return LongDateEditor(helper, fieldName, value, htmlAttributes);
        }
    }
}

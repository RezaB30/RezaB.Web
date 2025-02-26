﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Threading;
using System.Web.Mvc.Html;
using System.Linq.Expressions;

namespace RezaB.Web.Helpers
{
    public static class LongDateHelper
    {
        public static MvcHtmlString LongDateEditor(this HtmlHelper helper, string propName, DateTime? selectedDate = null, object htmlAttributes = null)
        {
            var hasValue = selectedDate != null && selectedDate.Value.Ticks > 0;
            var stringValue = hasValue ? selectedDate.Value.ToShortDateString() : string.Empty;
            var attributes = htmlAttributes != null ? htmlAttributes.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(htmlAttributes)) : new Dictionary<string, object>();
            attributes["placeholder"] = DateTime.Now.ToLongDateString();
            attributes["autocomplete"] = "off";
            return helper.TextBox(propName, stringValue, attributes);
        }

        public static MvcHtmlString LongDateEditorFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var value = metadata.Model as DateTime?;

            return LongDateEditor(helper, fieldName, value, htmlAttributes);
        }
    }
}

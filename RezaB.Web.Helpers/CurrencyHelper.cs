using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace RezaB.Web.Helpers
{
    public static class CurrencyHelper
    {
        public static MvcHtmlString CurrencyEditorFor<TModel,TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel,TResult>> expression)
        {
            TagBuilder wrapper = new TagBuilder("div");
            wrapper.AddCssClass("currency-input-wrapper");

            wrapper.InnerHtml = helper.TextBoxFor(model => model, new { @class = "currency-editor", @autocomplete = "off", @maxlength = 13 }).ToHtmlString();
            TagBuilder symbolSpan = new TagBuilder("span");
            symbolSpan.AddCssClass("currency-symbol");
            symbolSpan.InnerHtml = NumberFormatInfo.GetInstance(CultureInfo.GetCultureInfo("tr-tr")).CurrencySymbol;
            wrapper.InnerHtml += symbolSpan.ToString(TagRenderMode.Normal);

            return new MvcHtmlString(wrapper.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString CurrencyDisplayFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var value = metadata.Model as string;

            if (string.IsNullOrEmpty(value))
                return new MvcHtmlString("-");

            TagBuilder symbolSpan = new TagBuilder("span");
            symbolSpan.AddCssClass("currency-symbol");
            symbolSpan.InnerHtml = NumberFormatInfo.GetInstance(CultureInfo.GetCultureInfo("tr-tr")).CurrencySymbol;

            return new MvcHtmlString(value + " " + symbolSpan.ToString(TagRenderMode.Normal));
        }
    }
}

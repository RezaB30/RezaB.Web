using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace RezaB.Web.Helpers
{
    public static class CheckButtonHelper
    {
        /// <summary>
        /// Makes a button with ability to be checked.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">The model property name</param>
        /// <param name="IsChecked">If it is selected</param>
        /// <returns></returns>
        public static MvcHtmlString CheckButton(this HtmlHelper helper, string id, string text, bool IsChecked = false)
        {
            //var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(id);

            var container = new TagBuilder("div");
            container.AddCssClass("checkbutton-container");

            var checkbox = new TagBuilder("input");
            checkbox.MergeAttribute("type", "checkbox");
            checkbox.GenerateId(id);
            checkbox.MergeAttribute("name",  id);
            checkbox.MergeAttribute("style", "display: none;");
            if (IsChecked)
            {
                checkbox.MergeAttribute("checked", "checked");
            }

            var button = new TagBuilder("input");
            button.MergeAttribute("type", "button");
            button.MergeAttribute("value", HttpUtility.HtmlDecode(text));
            button.AddCssClass("link-button iconed-button check-button");
            if (IsChecked)
            {
                button.AddCssClass("selected");
            }

            container.InnerHtml = checkbox.ToString(TagRenderMode.SelfClosing) +
                button.ToString(TagRenderMode.SelfClosing);

            return new MvcHtmlString(container.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString CheckButton<TModel,TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, string text = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var name = text ?? metadata.DisplayName;

            return helper.CheckButton(fullName, name, metadata.Model as bool? ?? false);
        }
    }
}
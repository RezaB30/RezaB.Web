using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace RezaB.Web.Helpers
{
    public static class SelectHelper
    {
        /// <summary>
        /// Makes a selection list with search function
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">The model property name</param>
        /// <param name="list">List of items to show</param>
        /// <param name="firstOption">The first option text with null value (e. select...)</param>
        /// <param name="listPageUrl">Url to the list search page</param>
        /// <returns></returns>
        public static MvcHtmlString Select(this HtmlHelper helper, string id, SelectList list, string firstOption = null, string listPageUrl = null, object htmlAttributes = null)
        {
            UrlHelper Url = new UrlHelper(helper.ViewContext.RequestContext ,helper.RouteCollection);
            var wrapper = new TagBuilder("div");
            //wrapper.MergeAttribute("tabindex", "0");
            if (htmlAttributes != null)
                wrapper.MergeAdditionalAttributes(htmlAttributes);
            wrapper.AddCssClass("select-list-wrapper");

            var dropdownArrow = new TagBuilder("img");
            dropdownArrow.MergeAttribute("src", Url.Content( "~/Content/Images/Buttons/drop-down-arrow.svg"));
            dropdownArrow.AddCssClass("drop-down-arrow");

            var options = new TagBuilder("div");
            options.AddCssClass("options-container");
            //options.MergeAttribute("tabindex", "-1");
            var scrollWrapper = new TagBuilder("div");
            //scrollWrapper.MergeAttribute("tabindex", "-1");
            scrollWrapper.MergeAttribute("style", "position: relative;");

            var hidden = new TagBuilder("input");
            hidden.MergeAttribute("type", "hidden");
            hidden.GenerateId(id);
            hidden.MergeAttribute("name", id);

            var textbox = new TagBuilder("input");
            textbox.MergeAttribute("type", "text");
            //textbox.MergeAttribute("readonly", "");
            //textbox.MergeAttribute("tabindex", "-1");

            if (!string.IsNullOrEmpty(firstOption))
            {
                var firstTag = new TagBuilder("div");
                firstTag.MergeAttribute("value", "");
                firstTag.AddCssClass("list-option");
                if (list.SelectedValue == null)
                {
                    firstTag.AddCssClass("selected");
                    textbox.MergeAttribute("value", "", true);
                }
                firstTag.SetInnerText(firstOption);
                scrollWrapper.InnerHtml += firstTag.ToString(TagRenderMode.Normal);
                textbox.MergeAttribute("value", firstOption);
                hidden.MergeAttribute("value", "");
            }
            foreach (var item in list)
            {
                var currentTag = new TagBuilder("div");
                currentTag.AddCssClass("list-option");
                currentTag.MergeAttribute("value", item.Value ?? item.Text);
                currentTag.SetInnerText(item.Text);
                if (item.Selected)
                {
                    currentTag.AddCssClass("selected");
                    textbox.MergeAttribute("value", item.Text, true);
                    hidden.MergeAttribute("value", item.Value ?? item.Text, true);
                }
                scrollWrapper.InnerHtml += currentTag.ToString(TagRenderMode.Normal);
            }
            options.InnerHtml = scrollWrapper.ToString(TagRenderMode.Normal);

            wrapper.InnerHtml = dropdownArrow.ToString(TagRenderMode.SelfClosing) +
                hidden.ToString(TagRenderMode.SelfClosing) +
                textbox.ToString(TagRenderMode.SelfClosing) +
                options.ToString(TagRenderMode.Normal);

            TagBuilder listButton = null;
            if (!string.IsNullOrEmpty(listPageUrl))
            {
                //list button
                listButton = new TagBuilder("a");
                listButton.AddCssClass("link-button iconed-button list-button");
                listButton.MergeAttribute("href", listPageUrl);
                listButton.InnerHtml = Localization.Common.List;
            }

            return MvcHtmlString.Create(wrapper.ToString(TagRenderMode.Normal) + (listButton == null ? "" : "&nbsp;" + listButton.ToString(TagRenderMode.Normal)));
        }

        public static MvcHtmlString Select<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, SelectList list, string firstOption = null, string listPageUrl = null, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

            return helper.Select(fullName, list, firstOption, listPageUrl, htmlAttributes);
        }
    }
}
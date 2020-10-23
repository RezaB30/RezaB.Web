using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace RezaB.Web.Helpers
{
    public static class MultiSelectListHelper
    {
        public static MvcHtmlString MultiSelect(this HtmlHelper helper, string id, MultiSelectList list, string firstOption = null, string listPageUrl = null)
        {
            var selectedOptions = "";
            if (list.SelectedValues != null)
            {
                selectedOptions = string.Join(",", list.Where(item => item.Selected).Select(item => item.Value.ToString()).ToArray());
                //list = new SelectList(list.Items, null);
            }

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("multiselect-container");

            var singleSelect = helper.Select(id, new SelectList( list.Items, list.DataValueField, list.DataTextField), firstOption, listPageUrl);
            var sampleContainer = new TagBuilder("div");
            sampleContainer.MergeAttribute("style", "display: none;");
            sampleContainer.GenerateId(id);
            sampleContainer.AddCssClass("multiselect-sample");
            sampleContainer.MergeAttribute("selected_values", selectedOptions);

            var removeButton = new TagBuilder("input");
            removeButton.MergeAttribute("type", "button");
            removeButton.AddCssClass("link-button iconed-button remove-instance-button");
            removeButton.MergeAttribute("value", Localization.Common.Remove);

            sampleContainer.InnerHtml = singleSelect.ToString() +
                "&nbsp;" +
                removeButton.ToString(TagRenderMode.SelfClosing);

            var addInstanceButton = new TagBuilder("input");
            addInstanceButton.MergeAttribute("type", "button");
            addInstanceButton.AddCssClass("link-button iconed-button add-instance-button");
            addInstanceButton.MergeAttribute("value", Localization.Common.AddInstance);

            wrapper.InnerHtml =
                sampleContainer.ToString(TagRenderMode.Normal) +
                addInstanceButton.ToString(TagRenderMode.SelfClosing);

            return new MvcHtmlString(wrapper.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString MultiSelectFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel,TResult>> expression, MultiSelectList SelectedValues, string firstOption = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var model = metadata.Model;

            return helper.MultiSelect(fullName, SelectedValues, firstOption);
        }
    }
}
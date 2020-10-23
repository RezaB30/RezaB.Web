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
    public static class TextListEditorHelper
    {
        public static MvcHtmlString TextListEditor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression, object htmlAttributes = null) where TResult : IEnumerable<string>
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var Model = metadata.Model as IEnumerable<string> ?? new List<string>();

            var oldPrefix = helper.ViewData.TemplateInfo.HtmlFieldPrefix;
            if (string.IsNullOrEmpty(oldPrefix))
                helper.ViewData.TemplateInfo.HtmlFieldPrefix = fieldName;

            TagBuilder container = new TagBuilder("div");
            container.AddCssClass("text-list-editor-container");

            TagBuilder sampleDiv = new TagBuilder("div");
            sampleDiv.MergeAttribute("selected_values", string.Join(",", Model));
            sampleDiv.MergeAttribute("style", "display: none;");
            sampleDiv.AddCssClass("text-list-editor-sample");
            sampleDiv.InnerHtml = helper.TextBox(name: string.Empty, value: string.Empty, htmlAttributes: htmlAttributes).ToHtmlString();
            {
                var removeButton = new TagBuilder("input");
                removeButton.MergeAttribute("type", "button");
                removeButton.AddCssClass("link-button iconed-button remove-instance-button");
                removeButton.MergeAttribute("value", Localization.Common.Remove);
                sampleDiv.InnerHtml += "&nbsp;" + removeButton.ToString(TagRenderMode.SelfClosing);
            }
            container.InnerHtml += sampleDiv.ToString(TagRenderMode.Normal);

            //TagBuilder list = new TagBuilder("ol");
            //list.AddCssClass("multiselect-orderedlist");
            //for (int i = 0; i < Model.Count(); i++)
            //{
            //    TagBuilder listItem = new TagBuilder("li");
            //    listItem.AddCssClass("multiselect-input-row");
            //    listItem.InnerHtml = helper.TextBox(name: "[" + i + "]", value: Model.ToArray()[i], htmlAttributes: htmlAttributes).ToHtmlString();
            //    {
            //        var removeButton = new TagBuilder("input");
            //        removeButton.MergeAttribute("type", "button");
            //        removeButton.AddCssClass("link-button iconed-button remove-instance-button");
            //        removeButton.MergeAttribute("value", Localization.Common.Remove);
            //        listItem.InnerHtml += "&nbsp;" + removeButton.ToString(TagRenderMode.SelfClosing);
            //    }
            //    list.InnerHtml += listItem.ToString(TagRenderMode.Normal);
            //}
            //container.InnerHtml += list.ToString(TagRenderMode.Normal);

            var addInstanceButton = new TagBuilder("input");
            addInstanceButton.MergeAttribute("type", "button");
            addInstanceButton.AddCssClass("link-button iconed-button add-instance-button");
            addInstanceButton.MergeAttribute("value", Localization.Common.AddInstance);
            container.InnerHtml += addInstanceButton.ToString(TagRenderMode.SelfClosing);


            helper.ViewData.TemplateInfo.HtmlFieldPrefix = oldPrefix;

            return new MvcHtmlString(container.ToString(TagRenderMode.Normal));

        }
    }
}

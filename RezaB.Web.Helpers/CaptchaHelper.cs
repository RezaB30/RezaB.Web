using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace RezaB.Web.Helpers
{
    public static class CaptchaHelper
    {
        /// <summary>
        /// Generates captcha.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="fieldName">Name of captcha key field.</param>
        /// <param name="showValidationMessage">If validation message must be shown.</param>
        /// <returns></returns>
        public static MvcHtmlString Captcha(this HtmlHelper helper, string fieldName, string imageUrl, bool? showValidationMessage = false, object htmlAttributes = null)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);

            TagBuilder wrapper = new TagBuilder("div");
            wrapper.AddCssClass("captcha");

            TagBuilder imageWrapper = new TagBuilder("div");
            imageWrapper.AddCssClass("captcha-image-wrapper");

            TagBuilder image = new TagBuilder("img");
            image.MergeAttribute("src", imageUrl);
            image.GenerateId("captcha");

            TagBuilder reloadButton = new TagBuilder("input");
            reloadButton.MergeAttribute("type", "button");
            reloadButton.AddCssClass("captcha-reload-button");

            imageWrapper.InnerHtml = image.ToString(TagRenderMode.SelfClosing)
                + reloadButton.ToString(TagRenderMode.SelfClosing);

            TagBuilder captchaText = new TagBuilder("input");
            captchaText.MergeAttribute("type", "text");
            captchaText.GenerateId(fieldName);
            captchaText.MergeAttribute("name", fieldName);

            if (htmlAttributes != null)
            {
                foreach (var propertyInfo in htmlAttributes.GetType().GetProperties())
                {
                    captchaText.MergeAttribute(propertyInfo.Name, propertyInfo.GetValue(htmlAttributes).ToString());
                }
            }

            TagBuilder breakTag = new TagBuilder("br");

            TagBuilder validationMessage = new TagBuilder("span");
            validationMessage.AddCssClass("text-danger");
            validationMessage.SetInnerText(Localization.Common.CaptchaError);
            if (!showValidationMessage.HasValue || !showValidationMessage.Value)
            {
                validationMessage.MergeAttribute("style", "visibility: hidden;");
            }

            wrapper.InnerHtml = imageWrapper.ToString(TagRenderMode.Normal) +
                captchaText.ToString(TagRenderMode.SelfClosing) + breakTag.ToString(TagRenderMode.SelfClosing) +
                validationMessage.ToString(TagRenderMode.Normal);

            return new MvcHtmlString(wrapper.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString CaptchaFor<TModel, Tresult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, Tresult>> expression, string imageUrl, bool? showValidationMessage = false, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

            return helper.Captcha(fullName, imageUrl, showValidationMessage, htmlAttributes);
        }
    }
}
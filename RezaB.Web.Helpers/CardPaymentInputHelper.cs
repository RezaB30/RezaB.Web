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
    public static class CardPaymentInputHelper
    { 
        public static MvcHtmlString CardPaymentInputFor<TModel,TCardResults,TExpMonthResults,TExpYearResults,TCVVResults>(this HtmlHelper<TModel> helper, Expression<Func<TModel,TCardResults>> cardNoExpression, Expression<Func<TModel, TExpMonthResults>> expirationMonthExpression, Expression<Func<TModel, TExpYearResults>> expirationYearExpression, Expression<Func<TModel, TCVVResults>> cvvExpression)
        {
            // retrieving card meta
            var cardMetadata = ModelMetadata.FromLambdaExpression(cardNoExpression, helper.ViewData);
            var cardFieldName = ExpressionHelper.GetExpressionText(cardNoExpression);
            var cardFullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(cardFieldName);
            var cardValue = cardMetadata.Model;
            // retrieving exp month meta
            var expMonthMetadata = ModelMetadata.FromLambdaExpression(expirationMonthExpression, helper.ViewData);
            var expMonthFieldName = ExpressionHelper.GetExpressionText(expirationMonthExpression);
            var expMonthFullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expMonthFieldName);
            var expMonthValue = expMonthMetadata.Model;
            // retrieving card meta
            var expYearMetadata = ModelMetadata.FromLambdaExpression(expirationYearExpression, helper.ViewData);
            var expYearFieldName = ExpressionHelper.GetExpressionText(expirationYearExpression);
            var expYearFullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expYearFieldName);
            var expYearValue = expYearMetadata.Model;
            // retrieving card meta
            var cvvMetadata = ModelMetadata.FromLambdaExpression(cvvExpression, helper.ViewData);
            var cvvFieldName = ExpressionHelper.GetExpressionText(cvvExpression);
            var cvvFullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(cvvFieldName);
            var cvvValue = cvvMetadata.Model;

            TagBuilder wrapper = new TagBuilder("div");
            wrapper.AddCssClass("card-payment-wrapper");
            // create inputs
            wrapper.InnerHtml += helper.EditorFor(cardNoExpression, new { @htmlAttributes = new { @autocomplete="off", @maxlength = "19", @placeholder= helper.DisplayNameTextFor(cardNoExpression) } });
            wrapper.InnerHtml += helper.Select(expirationMonthExpression, HelperUtilities.CreateNumericSelectList(1,12, string.IsNullOrEmpty(expMonthValue as string) ? (int?)null : Convert.ToInt32(expMonthValue)));
            wrapper.InnerHtml += helper.Select(expirationYearExpression, HelperUtilities.CreateNumericSelectList(DateTime.Now.Year % 100, DateTime.Now.AddYears(10).Year % 100, string.IsNullOrEmpty(expYearValue as string) ? (int?)null : Convert.ToInt32(expYearValue)));
            //wrapper.InnerHtml += helper.EditorFor(cardNoExpression, new { @htmlAttributes = new { @autocomplete="off" } });
            //wrapper.InnerHtml += helper.EditorFor(cardNoExpression, new { @htmlAttributes = new { @autocomplete="off" } });
            wrapper.InnerHtml += helper.EditorFor(cvvExpression, new { @htmlAttributes = new { @autocomplete = "off", @maxlength = "3", @placeholder = helper.DisplayNameTextFor(cvvExpression) } });
            // create validations
            TagBuilder validationDiv = new TagBuilder("div");
            validationDiv.AddCssClass("validations-container");

            {
                TagBuilder currentValidationWrapper = new TagBuilder("div");
                currentValidationWrapper.InnerHtml += helper.ValidationMessageFor(cardNoExpression);
                validationDiv.InnerHtml += currentValidationWrapper.ToString(TagRenderMode.Normal);
            }
            {
                TagBuilder currentValidationWrapper = new TagBuilder("div");
                currentValidationWrapper.InnerHtml += helper.ValidationMessageFor(expirationMonthExpression);
                validationDiv.InnerHtml += currentValidationWrapper.ToString(TagRenderMode.Normal);
            }
            {
                TagBuilder currentValidationWrapper = new TagBuilder("div");
                currentValidationWrapper.InnerHtml += helper.ValidationMessageFor(expirationYearExpression);
                validationDiv.InnerHtml += currentValidationWrapper.ToString(TagRenderMode.Normal);
            }
            {
                TagBuilder currentValidationWrapper = new TagBuilder("div");
                currentValidationWrapper.InnerHtml += helper.ValidationMessageFor(cvvExpression);
                validationDiv.InnerHtml += currentValidationWrapper.ToString(TagRenderMode.Normal);
            }

            wrapper.InnerHtml += validationDiv.ToString(TagRenderMode.Normal);

            return new MvcHtmlString(wrapper.ToString(TagRenderMode.Normal));
        }
    }
}

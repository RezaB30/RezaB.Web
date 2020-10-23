using RezaB.Web.Helpers.HelperObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace RezaB.Web.Helpers
{
    public static class MonthPickerHelper
    {
        public static MvcHtmlString MonthPickerFor<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression) where TResult : MonthOfYear
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var value = (MonthOfYear)metadata.Model ?? new MonthOfYear();
            var datetimeInfo = DateTimeFormatInfo.GetInstance(Thread.CurrentThread.CurrentCulture);

            // wrapper
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("month-picker-wrapper");
            wrapper.MergeAttribute("tabindex", "0");
            wrapper.MergeAttribute("no-value-text", Localization.Common.Select);
            // hidden fields
            {
                var hidden = new TagBuilder("input");
                hidden.MergeAttribute("type", "hidden");
                hidden.MergeAttribute("name", fullName);
                hidden.GenerateId(fullName);
                if (value.IsValid)
                {
                    hidden.MergeAttribute("value", value.ToString());
                }
                wrapper.InnerHtml += hidden.ToString(TagRenderMode.SelfClosing);
            }
            // main button
            {
                var button = new TagBuilder("input");
                button.MergeAttribute("type", "button");
                button.AddCssClass("link-button date-picker-button");
                if (value.IsValid)
                {
                    button.MergeAttribute("value", value.Year + " " + datetimeInfo.GetMonthName(value.Month.Value));
                }
                else
                {
                    button.MergeAttribute("value", Localization.Common.Select);
                }
                wrapper.InnerHtml += button.ToString(TagRenderMode.SelfClosing);
            }
            // clear button
            {
                var clearButton = new TagBuilder("input");
                clearButton.MergeAttribute("type", "button");
                clearButton.AddCssClass("link-button");
                clearButton.AddCssClass("iconed-button");
                clearButton.AddCssClass("clear-button");
                wrapper.InnerHtml += clearButton.ToString(TagRenderMode.SelfClosing);
            }
            // picker
            {
                var popupContainer = new TagBuilder("div");
                popupContainer.MergeAttribute("style", "display: none;");
                popupContainer.AddCssClass("date-picker-popup");

                var calendar = new TagBuilder("div");
                calendar.MergeAttribute("style", "position: relative;");

                var monthsLayer = new TagBuilder("div");
                monthsLayer.MergeAttribute("tabindex", "0");
                monthsLayer.AddCssClass("calendar-months");
                {
                    var table = new TagBuilder("table");
                    {
                        var tableHead = new TagBuilder("tr");
                        var tableHeadCell = new TagBuilder("th");
                        tableHeadCell.MergeAttribute("colspan", "100%");
                        var prevButton = new TagBuilder("input");
                        prevButton.MergeAttribute("type", "button");
                        prevButton.MergeAttribute("value", "<");
                        prevButton.AddCssClass("calendar-previous-button");

                        var year = new TagBuilder("div");
                        year.AddCssClass("calendar-year");
                        year.MergeAttribute("tabindex", "0");
                        if (value != null && value.Year.HasValue)
                        {
                            year.InnerHtml = value.Year.ToString();
                        }
                        else
                        {
                            year.InnerHtml = DateTime.Now.Year.ToString();
                        }

                        var yearInput = new TagBuilder("input");
                        yearInput.AddCssClass("calendar-year-input");
                        yearInput.MergeAttribute("maxlength", "4");
                        yearInput.MergeAttribute("value", year.InnerHtml);

                        var nextButton = new TagBuilder("input");
                        nextButton.MergeAttribute("type", "button");
                        nextButton.MergeAttribute("value", ">");
                        nextButton.AddCssClass("calendar-next-button");

                        tableHeadCell.InnerHtml = prevButton.ToString(TagRenderMode.SelfClosing) +
                            "&nbsp;" +
                            year.ToString(TagRenderMode.Normal) + yearInput.ToString(TagRenderMode.SelfClosing) +
                            "&nbsp;" +
                            nextButton.ToString(TagRenderMode.SelfClosing);
                        tableHead.InnerHtml = tableHeadCell.ToString(TagRenderMode.Normal);
                        table.InnerHtml += tableHead.ToString(TagRenderMode.Normal);
                    }
                    for (int i = 1; i < 5; i++)
                    {
                        var tableRow = new TagBuilder("tr");
                        for (int j = 1; j < 4; j++)
                        {
                            var tableCell = new TagBuilder("td");
                            tableCell.MergeAttribute("tabindex", "0");
                            var monthNo = ((i - 1) * 3) + j;
                            tableCell.MergeAttribute("value", monthNo.ToString());
                            tableCell.InnerHtml = datetimeInfo.GetMonthName(monthNo);
                            tableRow.InnerHtml += tableCell.ToString(TagRenderMode.Normal);
                        }
                        table.InnerHtml += tableRow.ToString(TagRenderMode.Normal);
                    }
                    monthsLayer.InnerHtml += table.ToString(TagRenderMode.Normal);
                }
                calendar.InnerHtml += monthsLayer.ToString(TagRenderMode.Normal);

                popupContainer.InnerHtml += calendar.ToString(TagRenderMode.Normal);
                wrapper.InnerHtml += popupContainer.ToString(TagRenderMode.Normal);
            }

            return new MvcHtmlString(wrapper.ToString(TagRenderMode.Normal));
        }
    }
}
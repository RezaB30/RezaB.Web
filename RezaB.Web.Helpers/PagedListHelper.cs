using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RezaB.Web.Helpers
{
    public static class PagedListHelper
    {
        /// <summary>
        /// Makes a paging row for data tables
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="pageCount">Total number of pages in current table</param>
        /// <param name="pageNumber">Current data page</param>
        /// <returns></returns>
        public static MvcHtmlString PagedList(this HtmlHelper helper, int pageCount, int pagesLinkCount, int? pageNumber = null)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            //retrieve page number from query string
            int routePage;
            if (!pageNumber.HasValue)
            {
                try
                {
                    routePage = Convert.ToInt32(HttpContext.Current.Request.QueryString.Get("page"));
                }
                catch (Exception)
                {
                    routePage = 0;
                }
                pageNumber = routePage;
            }
            //Makes a div wrapper around the entire element
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("table-pages");
            //Make summary of pages
            var pagesSummary = new TagBuilder("span");
            pagesSummary.InnerHtml = string.Format(Localization.Common.PageSummary, pageNumber + 1, pageCount);
            //Make first, previous and ... elements
            var firstLink = new TagBuilder("a");
            firstLink.AddCssClass("page-link");
            firstLink.MergeAttribute("title", Localization.Common.First);
            firstLink.MergeAttribute("href", urlHelper.Current(new { page = 0 }, new string[] { "errorMessage" }).ToString());
            if (pageNumber == 0)
            {
                firstLink.AddCssClass("disabled");
                firstLink.MergeAttribute("href", "javascript:void(0);", true);
            }
            firstLink.InnerHtml = "<<";
            var prevLink = new TagBuilder("a");
            prevLink.AddCssClass("page-link");
            prevLink.MergeAttribute("title", Localization.Common.Previous);
            prevLink.MergeAttribute("href", urlHelper.Current(new { page = pageNumber - 1 }, new string[] { "errorMessage" }).ToString());
            if (pageNumber - 1 < 0)
            {
                prevLink.AddCssClass("disabled");
                prevLink.MergeAttribute("href", "javascript:void(0);", true);
            }
            prevLink.InnerHtml = "<";
            wrapper.InnerHtml += pagesSummary.ToString(TagRenderMode.Normal)
                + firstLink.ToString(TagRenderMode.Normal)
                + prevLink.ToString(TagRenderMode.Normal);
            if (pageNumber - pagesLinkCount > 0)
            {
                wrapper.InnerHtml += "...";
            }
            //Making page number links
            for (int i = pageNumber.Value - pagesLinkCount; i <= pageNumber.Value + pagesLinkCount; i++)
            {
                if (i >= 0 && i < pageCount)
                {
                    var pageLink = new TagBuilder("a");
                    pageLink.AddCssClass("page-link");
                    pageLink.MergeAttribute("href", urlHelper.Current(new { page = i }, new string[] { "errorMessage" }).ToString());
                    pageLink.InnerHtml = i + 1 + "";

                    if (i == pageNumber.Value)
                    {
                        pageLink.AddCssClass("disabled");
                        pageLink.MergeAttribute("href", "javascript:void(0);", true);
                    }

                    wrapper.InnerHtml += pageLink.ToString(TagRenderMode.Normal);
                }
            }
            //Make next, last and ... elements
            if (pageNumber + pagesLinkCount < pageCount - 1)
            {
                wrapper.InnerHtml += "...";
            }
            var nextLink = new TagBuilder("a");
            nextLink.AddCssClass("page-link");
            nextLink.MergeAttribute("title", Localization.Common.Next);
            nextLink.MergeAttribute("href", urlHelper.Current(new { page = pageNumber + 1 }, new string[] { "errorMessage" }).ToString());
            if (pageNumber + 1 >= pageCount)
            {
                nextLink.AddCssClass("disabled");
                nextLink.MergeAttribute("href", "javascript:void(0);", true);
            }
            nextLink.InnerHtml = ">";
            var lastLink = new TagBuilder("a");
            lastLink.AddCssClass("page-link");
            lastLink.MergeAttribute("title", Localization.Common.Last);
            lastLink.MergeAttribute("href", urlHelper.Current(new { page = pageCount - 1 }, new string[] { "errorMessage" }).ToString());
            if (pageNumber + 1 >= pageCount)
            {
                lastLink.AddCssClass("disabled");
                lastLink.MergeAttribute("href", "javascript:void(0);", true);
            }
            lastLink.InnerHtml = ">>";

            wrapper.InnerHtml += nextLink.ToString(TagRenderMode.Normal)
                + lastLink.ToString(TagRenderMode.Normal);

            //return the result
            return new MvcHtmlString(wrapper.ToString(TagRenderMode.Normal));
        }
    }
}
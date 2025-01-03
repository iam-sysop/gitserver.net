﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;
using System.Text;
using System.Web.Routing;
using System.Linq.Expressions;
using gitserverdotnet.Models;
using System.ComponentModel.DataAnnotations;

namespace gitserverdotnet.Helpers
{
    public static class CustomHtmlHelpers
    {
        public static IHtmlString AssemblyVersion(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        public static IHtmlString MarkdownToHtml(this HtmlHelper helper, string markdownText)
        {
            return MvcHtmlString.Create(CommonMark.CommonMarkConverter.Convert(markdownText));
        }

        public static MvcHtmlString DisplayEnum(this HtmlHelper helper, Enum e)
        {
            string result = "[[" + e.ToString() + "]]";
            var memberInfo = e.GetType().GetMember(e.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                var display = memberInfo.GetCustomAttributes(false)
                    .OfType<DisplayAttribute>()
                    .LastOrDefault();

                if (display != null)
                {
                    result = display.GetName();
                }
            }

            return MvcHtmlString.Create(result);
        }
    }
}

using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CorTabernaclChoir.Helpers
{
    public static class HtmlHelperExtensions
    {
        private const string BoldPattern = @"\[b\](.+?)\[\/b\]";
        private const string ItalicPattern = @"\[i\](.+?)\[\/i\]";
        private const string UrlPattern = @"([-a-z0-9._~:\/?#@!$&\'()*+,;=%]+)";
        private const string LinkPattern = @"\[url\]" + UrlPattern + @"\[\/url\]";
        private const string LinkWithTextPattern = @"\[url\=" + UrlPattern + @"\](.+?)\[\/url\]";
        private const string EmailAddressPattern = @"[-a-zA-Z0-9._]+@[-a-zA-Z0-9._]+[-a-zA-Z0-9_]+";

        public static MvcHtmlString DisplayFormattedFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var model = html.Encode(metadata.Model);
            model = model.Replace(Environment.NewLine, "<br />");
            
            model = Regex.Replace(model, BoldPattern, GetBold, RegexOptions.IgnoreCase);
            model = Regex.Replace(model, ItalicPattern, GetItalic, RegexOptions.IgnoreCase);
            model = Regex.Replace(model, LinkPattern, GetLink, RegexOptions.IgnoreCase);
            model = Regex.Replace(model, LinkWithTextPattern, GetLinkWithText, RegexOptions.IgnoreCase);
            model = Regex.Replace(model, EmailAddressPattern, GetEmailLink, RegexOptions.IgnoreCase);

            return string.IsNullOrEmpty(model)
                ? MvcHtmlString.Empty
                : MvcHtmlString.Create(model);
        }

        public static MvcHtmlString DisplayDateFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string format = "dd/MM/yyyy")
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var date = (DateTime)metadata.Model;
            var model = html.Encode(date.ToString(format));
            return MvcHtmlString.Create(model);
        }

        private static string GetBold(Match match)
        {
            var tag = new TagBuilder("strong");
            tag.SetInnerText(match.Groups[1].Value);
            var tagString = tag.ToString();
            return tagString;
        }

        private static string GetItalic(Match match)
        {
            var tag = new TagBuilder("em");
            tag.SetInnerText(match.Groups[1].Value);
            var tagString = tag.ToString();
            return tagString;
        }

        private static string GetLink(Match match)
        {
            var tag = new TagBuilder("a");
            var url = match.Groups[1].Value;
            tag.Attributes.Add("href", url);
            tag.Attributes.Add("target", "_blank");
            tag.SetInnerText(url);
            var tagString = tag.ToString();
            return tagString;
        }

        private static string GetLinkWithText(Match match)
        {
            var tag = new TagBuilder("a");
            tag.Attributes.Add("href", match.Groups[1].Value);
            tag.Attributes.Add("target", "_blank");
            tag.SetInnerText(match.Groups[2].Value);
            var tagString = tag.ToString();
            return tagString;
        }

        private static string GetEmailLink(Match match)
        {
            var tag = new TagBuilder("a");
            var email = match.Groups[0].Value;
            tag.Attributes.Add("href", $"mailto:{email}");
            tag.Attributes.Add("target", "_blank");
            tag.SetInnerText(email);
            var tagString = tag.ToString();
            return tagString;
        }

        public static MvcHtmlString DisplayEditButton(this HtmlHelper html, string url, object htmlAttributes = null)
        {
            return DisplayButton("pencil", "primary", "Edit", url, htmlAttributes);
        }

        public static MvcHtmlString DisplayAddButton(this HtmlHelper html, string url, object htmlAttributes = null)
        {
            return DisplayButton("plus-sign", "primary", "Add New", url, htmlAttributes);
        }

        public static MvcHtmlString DisplayDeleteButton(this HtmlHelper html, string url, object htmlAttributes = null)
        {
            return DisplayButton("trash", "danger", "Delete", url, htmlAttributes);
        }

        public static MvcHtmlString DisplaySaveButton(this HtmlHelper html, object htmlAttributes = null)
        {
            return DisplayButton("ok", "danger", "Save", null, htmlAttributes);
        }

        public static MvcHtmlString DisplayDeleteConfirmButton(this HtmlHelper html, object htmlAttributes = null)
        {
            return DisplayButton("ok", "danger", "Delete", null, htmlAttributes);
        }

        public static MvcHtmlString DisplayCancelButton(this HtmlHelper html, string url, object htmlAttributes = null)
        {
            return DisplayButton("remove", "default", "Cancel", url, htmlAttributes);
        }

        private static MvcHtmlString DisplayButton(string icon, string type, string text, string url = null, object htmlAttributes = null)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return MvcHtmlString.Empty;

            TagBuilder builder;

            if (!string.IsNullOrEmpty(url))
            {
                builder = new TagBuilder("a");

                builder.Attributes.Add("href", url);
            }
            else
            {
                builder = new TagBuilder("button");

                builder.Attributes.Add("type", "submit");
            }
            
            builder.InnerHtml = "<span class='glyphicon glyphicon-" + icon + "'></span> " + text;

            builder.AddCssClass("btn");
            builder.AddCssClass("btn-" + type);
            builder.AddCssClass("btn-sm");

            if (htmlAttributes != null)
            {
                var attributes = new RouteValueDictionary(htmlAttributes);

                if (attributes.ContainsKey("class"))
                {
                    builder.Attributes["class"] = builder.Attributes["class"] + " " + attributes["class"];
                }

                builder.MergeAttributes(attributes);
            }

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}
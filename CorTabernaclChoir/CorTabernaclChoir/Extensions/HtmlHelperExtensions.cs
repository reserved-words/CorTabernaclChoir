using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CorTabernaclChoir.Common.Models;
using CorTabernaclChoir.Common.ViewModels;

namespace CorTabernaclChoir.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString DisplayPostImage(this HtmlHelper html, PostImage image)
        {
            var tag = new TagBuilder("img");
            tag.MergeAttribute("src", $"/Images/Posts/{image.Id}{image.FileExtension}");
            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString DisplayPostImage(this HtmlHelper html, PostImageViewModel image)
        {
            var tag = new TagBuilder("img");
            tag.MergeAttribute("src", $"/Images/Posts/{image.Id}{image.FileExtension}");
            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString DisplayDateFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string format = "dd/MM/yyyy")
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var date = (DateTime)metadata.Model;
            var model = html.Encode(date.ToString(format));
            return MvcHtmlString.Create(model);
        }

        public static MvcHtmlString DisplayEditButton(this HtmlHelper html, string url, object htmlAttributes = null, string text = null)
        {
            return DisplayButton("pencil", "primary", text ?? "Edit", url, htmlAttributes);
        }

        public static MvcHtmlString DisplayAddButton(this HtmlHelper html, string url, object htmlAttributes = null, string text = null)
        {
            return DisplayButton("plus-sign", "primary", text ?? "Add New", url, htmlAttributes);
        }

        public static MvcHtmlString DisplayDeleteButton(this HtmlHelper html, string url, object htmlAttributes = null, string text = null)
        {
            return DisplayButton("trash", "danger", text ?? "Delete", url, htmlAttributes);
        }

        public static MvcHtmlString DisplaySaveButton(this HtmlHelper html, object htmlAttributes = null, string text = null, string type = null, string icon = null)
        {
            return DisplayButton(icon ?? "ok", type ?? "danger", text ?? "Save", null, htmlAttributes);
        }

        public static MvcHtmlString DisplayDeleteConfirmButton(this HtmlHelper html, object htmlAttributes = null, string text = null)
        {
            return DisplayButton("ok", "danger", text ?? "Delete", null, htmlAttributes);
        }

        public static MvcHtmlString DisplayCancelButton(this HtmlHelper html, string url, object htmlAttributes = null, string text = null)
        {
            return DisplayButton("remove", "default", text ?? "Cancel", url, htmlAttributes);
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
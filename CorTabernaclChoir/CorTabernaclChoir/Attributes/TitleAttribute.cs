using System.Web.Mvc;
using System.Web.Routing;
using static CorTabernaclChoir.Common.Resources;

namespace CorTabernaclChoir.Attributes
{
    public class TitleAttribute : ActionFilterAttribute
    {
        private const string RouteKeyCulture = "culture";

        public TitleAttribute(string titleResourceName, string menuResourceName = "")
        {
            TitleResourceName = titleResourceName;
            MenuResourceName = menuResourceName;
        }

        public string TitleResourceName { get; set; }
        public string MenuResourceName { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.Title = ResourceManager.GetString(TitleResourceName);
            filterContext.Controller.ViewBag.MenuTitle = ResourceManager.GetString(MenuResourceName);

            var culture = filterContext.RouteData.Values[RouteKeyCulture].ToString() == LanguageWelsh ? LanguageEnglish : LanguageWelsh;

            filterContext.Controller.ViewBag.TranslatedRoute = UrlHelper.GenerateUrl(
                null,
                filterContext.ActionDescriptor.ActionName,
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, 
                new RouteValueDictionary { { RouteKeyCulture, culture } },
                RouteTable.Routes, 
                filterContext.RequestContext,
                false);

            base.OnActionExecuted(filterContext);
        }
    }
}
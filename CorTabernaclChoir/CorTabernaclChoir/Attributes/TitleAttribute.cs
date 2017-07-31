using CorTabernaclChoir.Common;
using System.Web.Mvc;

namespace CorTabernaclChoir.Attributes
{
    public class TitleAttribute : ActionFilterAttribute
    {
        public TitleAttribute(string titleResourceName, string menuResourceName)
        {
            TitleResourceName = titleResourceName;
            MenuResourceName = menuResourceName;
        }

        public string TitleResourceName { get; set; }
        public string MenuResourceName { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.Title = Resources.ResourceManager.GetString(TitleResourceName);
            filterContext.Controller.ViewBag.MenuTitle = Resources.ResourceManager.GetString(MenuResourceName);

            base.OnActionExecuted(filterContext);
        }
    }
}
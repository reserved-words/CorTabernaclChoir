﻿using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CorTabernaclChoir
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string ErrorActionRoute = "/Error";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            // Log error
            Server.ClearError();
            Response.Redirect(ErrorActionRoute);
        }
    }
}

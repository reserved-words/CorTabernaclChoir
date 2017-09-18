using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorTabernaclChoir.Attributes
{
    public class RequireSecureConnectionAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException(nameof(filterContext));

            if (filterContext.RequestContext.HttpContext.Request.IsLocal)
                return;

            base.OnAuthorization(filterContext);
        }
    }
}
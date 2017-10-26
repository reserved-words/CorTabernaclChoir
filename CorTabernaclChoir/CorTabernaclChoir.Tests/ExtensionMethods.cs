using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace CorTabernaclChoir.Tests
{
    public static class ExtensionMethods
    {
        public static void Setup(this Controller controller, Uri requestUri)
        {
            var request = new Mock<HttpRequestBase>();
            request.SetupGet(x => x.Url).Returns(requestUri);
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
        }
    }
}

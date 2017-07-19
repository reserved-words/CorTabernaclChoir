using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CorTabernaclChoir.Startup))]
namespace CorTabernaclChoir
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

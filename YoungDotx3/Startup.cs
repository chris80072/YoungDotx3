using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YoungDotx3.Startup))]
namespace YoungDotx3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

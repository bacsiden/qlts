using Microsoft.Owin;
using Owin;
using StructureMap;

[assembly: OwinStartupAttribute(typeof(DK.Web.Startup))]
namespace DK.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kickit.Startup))]
namespace Kickit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

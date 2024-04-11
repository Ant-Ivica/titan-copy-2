using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FA.LVIS.Tower.UI.Startup))]
namespace FA.LVIS.Tower.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
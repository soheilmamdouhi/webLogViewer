using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(webLogViewer.Startup))]
namespace webLogViewer
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

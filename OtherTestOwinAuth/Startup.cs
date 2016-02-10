using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OtherTestOwinAuth.Startup))]
namespace OtherTestOwinAuth
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

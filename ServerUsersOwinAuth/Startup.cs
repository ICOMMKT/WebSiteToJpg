using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ServerUsersOwinAuth.Startup))]
namespace ServerUsersOwinAuth
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

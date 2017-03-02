using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(POSAccount.Startup))]
namespace POSAccount
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

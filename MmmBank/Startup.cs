using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MmmBank.Startup))]
namespace MmmBank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

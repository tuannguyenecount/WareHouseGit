using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WareHouse.Startup))]
namespace WareHouse
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

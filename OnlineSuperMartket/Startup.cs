using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineSuperMartket.Startup))]
namespace OnlineSuperMartket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

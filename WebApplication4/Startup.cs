using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CreateReferralTestHarness.Startup))]
namespace CreateReferralTestHarness
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CommunityProject_ProjectUnity.Startup))]
namespace CommunityProject_ProjectUnity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

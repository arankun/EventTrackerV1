#region directives

using Microsoft.Owin;
using Owin;

#endregion

[assembly: OwinStartup(typeof(EventTrackerAPI.Startup))]

namespace EventTrackerAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

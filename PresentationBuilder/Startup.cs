using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PresentationBuilder.Startup))]
namespace PresentationBuilder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

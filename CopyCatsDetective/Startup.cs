using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CopyCatsDetective.Startup))]
namespace CopyCatsDetective
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SozialHeap.Startup))]
namespace SozialHeap
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

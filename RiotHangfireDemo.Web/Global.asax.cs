using System.Web;

namespace RiotHangfireDemo
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Startup.Initialize();
        }
    };
}

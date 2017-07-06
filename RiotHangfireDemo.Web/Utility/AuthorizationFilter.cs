using Hangfire.Dashboard;
using Microsoft.Owin;

namespace RiotHangfireDemo.Web
{
    public class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
        {
            var owinEnvironment = dashboardContext.GetOwinEnvironment();
            var owinContext = new OwinContext(owinEnvironment);

            var isAuthenticated = owinContext
                                      .Authentication
                                      .User
                                      .Identity
                                      ?.IsAuthenticated == true;

            return isAuthenticated;
        }
    };
}

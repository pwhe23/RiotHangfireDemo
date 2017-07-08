using Hangfire.Dashboard;
using Microsoft.Owin;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    public class DemoHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
        {
            var owinEnvironment = dashboardContext.GetOwinEnvironment();
            var owinContext = new OwinContext(owinEnvironment);

            var userIsAdmin = owinContext
                .Authentication
                .User
                .IsInRole(User.ADMIN);

            return userIsAdmin;
        }
    };
}

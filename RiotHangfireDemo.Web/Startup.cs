using System.Web.Mvc;
using System.Web.Routing;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RiotHangfireDemo.Startup))]

namespace RiotHangfireDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureHangfire(app);
        }

        public static void Initialize()
        {
            CongigureEntityFramework();
            ConfigureMvc(RouteTable.Routes);
        }

        private static void CongigureEntityFramework()
        {
            using (var db = new DemoDb())
            {
                db.Database.CreateIfNotExists();
            }
        }

        private static void ConfigureMvc(RouteCollection routes)
        {
            AreaRegistration.RegisterAllAreas();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        private static void ConfigureHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("DemoDb");

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    };
}
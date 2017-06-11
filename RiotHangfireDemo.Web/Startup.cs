using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hangfire;
using MediatR;
using Microsoft.Owin;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;

[assembly: OwinStartup(typeof(RiotHangfireDemo.Startup))]

namespace RiotHangfireDemo
{
    public class Startup
    {
        private static readonly Container Container = new Container();
        public static string Version { get; } = Guid.NewGuid().ToString("N");

        public void Configuration(IAppBuilder app)
        {
            ConfigureSimpleInjector();
            ConfigureMediator();
            CongigureEntityFramework();
            ConfigureMvc(RouteTable.Routes);
            ConfigureHangfire(app);
            ConfigureSignalr(app);

            Container.Verify();

            Commander.Initialize(typeof(Startup).Assembly);
        }

        private static void ConfigureSimpleInjector()
        {
            Container.Options.DefaultScopedLifestyle = Lifestyle.CreateHybrid(() => HttpContext.Current == null,
                new ThreadScopedLifestyle(),
                new WebRequestLifestyle()
            );

            var services = typeof(Startup).Assembly.GetInterfacesWithSingleImplementation();
            foreach (var service in services)
            {
                Container.Register(service.Key, service.Value, Lifestyle.Scoped);
            }

            Container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
        }

        private static void ConfigureMediator()
        {
            var requestType = typeof(IRequestHandler<,>);
            var assemblies = new[] { typeof(Startup).Assembly };

            Container.Register(requestType, assemblies);
            Container.RegisterSingleton<IMediator>(() => new Mediator(Container.GetInstance, Container.GetAllInstances));
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
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container));
        }

        //REF: http://docs.hangfire.io/en/latest/quick-start.html
        private static void ConfigureHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(nameof(DemoDb))
                .UseActivator(new SimpleInjectorJobActivator(Container, Lifestyle.Scoped))
                .UseFilter(new HangfireJobPusher(Container.GetInstance<IPusher>()));

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 2,
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions());
        }

        private static void ConfigureSignalr(IAppBuilder app)
        {
            app.MapSignalR();
        }
    };
}
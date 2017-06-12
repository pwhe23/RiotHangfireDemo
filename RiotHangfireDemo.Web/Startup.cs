using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hangfire;
using MediatR;
using Microsoft.Owin;
using Owin;
using RiotHangfireDemo.Domain;
using RiotHangfireDemo.Web;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;

[assembly: OwinStartup(typeof(Startup))]

namespace RiotHangfireDemo.Web
{
    public class Startup
    {
        private static readonly Container Container = new Container();
        private static readonly Assembly[] AppAssemblies =
        {
            typeof(Startup).Assembly,
            typeof(DemoConfig).Assembly,
        };

        public static string Version { get; } = Guid.NewGuid().ToString("N"); //browser cachebuster

        public void Configuration(IAppBuilder app)
        {
            ConfigureSimpleInjector();
            ConfigureSettings();
            ConfigureMediator();
            CongigureEntityFramework();
            ConfigureMvc(RouteTable.Routes);
            ConfigureHangfire(app);
            ConfigureSignalr(app);

            Container.Verify();

            Commander.Initialize(AppAssemblies);
        }

        private static void ConfigureSimpleInjector()
        {
            Container.Options.DefaultScopedLifestyle = Lifestyle.CreateHybrid(() => HttpContext.Current == null,
                new ThreadScopedLifestyle(),
                new WebRequestLifestyle()
            );

            var services = Ext.GetInterfacesWithSingleImplementation(AppAssemblies);
            foreach (var service in services)
            {
                Container.Register(service.Key, service.Value, Lifestyle.Scoped);
            }

            Container.Register<IDb, DemoDb>(Lifestyle.Scoped); //InternalsVisibleTo

            Container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
        }

        private static void ConfigureSettings()
        {
            // Put strongly-typed configuration classes in the container for injection
            var config = Ext.MapAppSettingsToClass<DemoConfig>();
            Container.RegisterSingleton(config);
        }

        private static void ConfigureMediator()
        {
            var requestType = typeof(IRequestHandler<,>);

            Container.Register(requestType, AppAssemblies);
            Container.RegisterSingleton<IMediator>(() => new Mediator(Container.GetInstance, Container.GetAllInstances));
        }

        private static void CongigureEntityFramework()
        {
            var db = Container.GetInstance<IDb>();
            db.CreateDatabase();
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

        private static void ConfigureHangfire(IAppBuilder app)
        {
            var config = Container.GetInstance<DemoConfig>();

            //REF: http://docs.hangfire.io/en/latest/quick-start.html
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(nameof(DemoDb))
                .UseActivator(new SimpleInjectorJobActivator(Container, Lifestyle.Scoped))
                .UseFilter(new HangfireJobPusher(Container.GetInstance<IPusher>()));

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = config.HangfireWorkerCount,
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions());
        }

        private static void ConfigureSignalr(IAppBuilder app)
        {
            app.MapSignalR();
        }
    };
}
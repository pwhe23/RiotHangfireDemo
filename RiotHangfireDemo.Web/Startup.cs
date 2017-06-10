using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hangfire;
using Hangfire.SimpleInjector;
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
        public static Container Container { get; set; }
        public static long Version { get; } = DateTime.UtcNow.Ticks;

        public void Configuration(IAppBuilder app)
        {
            Container = ConfigureSimpleInjector();

            ConfigureMediator(Container);
            CongigureEntityFramework();
            ConfigureMvc(Container, RouteTable.Routes);
            ConfigureHangfire(app, Container);

            Container.Verify();
        }

        private static Container ConfigureSimpleInjector()
        {
            var container = new Container();

            var lifestyle = Lifestyle.CreateHybrid(() => HttpContext.Current == null,
                new ThreadScopedLifestyle(),
                new WebRequestLifestyle()
            );

            foreach (var service in GetInterfacesWithSingleImplementation(typeof(Startup).Assembly))
            {
                container.Register(service.Key, service.Value, lifestyle);
            }

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            return container;
        }

        private static void ConfigureMediator(Container container)
        {
            var requestType = typeof(IRequestHandler<,>);
            var assemblies = new[] { typeof(Startup).Assembly };

            container.Register(requestType, assemblies);
            container.RegisterSingleton<IMediator>(() => new Mediator(container.GetInstance, container.GetAllInstances));
        }

        private static void CongigureEntityFramework()
        {
            using (var db = new DemoDb())
            {
                db.Database.CreateIfNotExists();
            }
        }

        private static void ConfigureMvc(Container container, RouteCollection routes)
        {
            AreaRegistration.RegisterAllAreas();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        //REF: http://docs.hangfire.io/en/latest/quick-start.html
        private static void ConfigureHangfire(IAppBuilder app, Container container)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(nameof(DemoDb))
                .UseActivator(new SimpleInjectorJobActivator(container));

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }

        private static Dictionary<Type, Type> GetInterfacesWithSingleImplementation(Assembly assembly)
        {
            return assembly
                .GetExportedTypes()
                .Where(x => x.IsClass
                            && !x.IsAbstract)
                .SelectMany(x => x
                    .GetInterfaces()
                    .Where(i => i.Assembly == assembly)
                    .Select(i => new
                    {
                        Implementation = x,
                        Interface = i,
                    })
                )
                .GroupBy(x => x.Interface, (k, g) => new
                {
                    Interface = k,
                    Implemenations = g.Select(y => y.Implementation).ToArray(),
                })
                .Where(x => x.Implemenations.Length == 1)
                .ToDictionary(x => x.Interface, x => x.Implemenations[0]);
        }
    };
}
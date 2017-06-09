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

        public void Configuration(IAppBuilder app)
        {
            ConfigureHangfire(app, Container);
        }

        public static void Initialize()
        {
            Container = ConfigureSimpleInjector();

            ConfigureMediatr(Container);
            CongigureEntityFramework();
            ConfigureMvc(Container, RouteTable.Routes);

            Container.Verify();
        }

        private static Container ConfigureSimpleInjector()
        {
            var container = new Container();

            var lifestyle = Lifestyle.CreateHybrid(() => HttpContext.Current == null,
                new ThreadScopedLifestyle(),
                new WebRequestLifestyle()
            );

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.Register<DemoDb>(lifestyle);
            container.Register<Queue>();

            return container;
        }

        private static void ConfigureMediatr(Container container)
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
    };
}
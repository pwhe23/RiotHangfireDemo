using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Hangfire;
using MediatR;
using Microsoft.Owin;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

[assembly: OwinStartup(typeof(RiotHangfireDemo.Startup))]

namespace RiotHangfireDemo
{
    public class Startup
    {
        private static Container _container;

        public void Configuration(IAppBuilder app)
        {
            ConfigureHangfire(app);
        }

        public static void Initialize()
        {
            _container = ConfigureSimpleInjector();

            ConfigureMediatr();
            CongigureEntityFramework();
            ConfigureMvc(RouteTable.Routes);

            _container.Verify();
        }

        private static Container ConfigureSimpleInjector()
        {
            var container = new Container();
            var lifestyle = new WebRequestLifestyle();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.Register<DemoDb>(lifestyle);

            return container;
        }

        private static void ConfigureMediatr()
        {
            var requestType = typeof(IRequestHandler<,>);
            var assemblies = new[] { typeof(Startup).Assembly };

            _container.Register(requestType, assemblies);
            _container.RegisterSingleton<IMediator>(() => new Mediator(_container.GetInstance, _container.GetAllInstances));
            _container.RegisterCollection(typeof(IPipelineBehavior<,>), Enumerable.Empty<Type>());
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

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(_container));
        }

        //REF: http://docs.hangfire.io/en/latest/quick-start.html
        private static void ConfigureHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(nameof(DemoDb));

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    };
}
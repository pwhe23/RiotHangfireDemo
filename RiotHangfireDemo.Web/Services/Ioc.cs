using System;
using SimpleInjector.Lifestyles;

namespace RiotHangfireDemo
{
    public static class Ioc
    {
        public static T Get<T>() where T : class
        {
            return Startup.Container.GetInstance<T>();
        }

        public static IDisposable BeginScope()
        {
            return ThreadScopedLifestyle.BeginScope(Startup.Container);
        }
    };
}
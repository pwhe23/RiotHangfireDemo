using System;
using Hangfire;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Lifestyles;

namespace RiotHangfireDemo.Web
{
    //REF: https://github.com/devmondo/HangFire.SimpleInjector/blob/eec5646c321225e76ea1eb06f01a69e60a273a7f/src/HangFire.SimpleInjector/SimpleInjectorJobActivator.cs
    /// <summary>
    /// I couldn't get the HangFire.SimpleInjector Nuget package to work with the new
    /// ThreadScopedLifestyle so I copied the source here and fixed it.
    /// </summary>
    public class SimpleInjectorJobActivator : JobActivator
    {
        private readonly Container _container;
        private readonly Lifestyle _lifestyle;

        public SimpleInjectorJobActivator(Container container)
        {
            _container = container;
        }

        public SimpleInjectorJobActivator(Container container, Lifestyle lifestyle)
        {
            _container = container;
            _lifestyle = lifestyle;
        }

        public override object ActivateJob(Type jobType)
        {
            return _container.GetInstance(jobType);
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            if (_lifestyle == null)
            {
                return new SimpleInjectorScope(_container, _container.BeginExecutionContextScope());
            }
            else if (_lifestyle == Lifestyle.Scoped)
            {
                return new SimpleInjectorScope(_container, ThreadScopedLifestyle.BeginScope(_container));
            }

            return new SimpleInjectorScope(_container, _container.GetCurrentExecutionContextScope());
        }

        internal class SimpleInjectorScope : JobActivatorScope
        {
            private readonly Container _container;
            private readonly Scope _scope;

            public SimpleInjectorScope(Container container, Scope scope)
            {
                _container = container;
                _scope = scope;
            }

            public override object Resolve(Type type)
            {
                return _container.GetInstance(type);
            }

            public override void DisposeScope()
            {
                _scope?.Dispose();
            }
        };
    };
}
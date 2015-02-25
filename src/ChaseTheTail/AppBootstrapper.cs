using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using StructureMap;

namespace ChaseTheTail
{
    public class AppBootstrapper : BootstrapperBase
    {
        Container _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new Container();

            _container.Configure(x =>
            {
                x.ForSingletonOf<IWindowManager>().Use<WindowManager>();
                x.ForSingletonOf<IEventAggregator>().Use<EventAggregator>();
                x.For<IShell>().Use<ShellViewModel>();
            });
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = String.IsNullOrEmpty(key)
                ? _container.GetInstance(service)
                : _container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service).OfType<object>();
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Platform.VirtualFileSystem;
using ReactiveUI;
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
                x.ForSingletonOf<IFileSystemManager>().Use(FileSystemManager.Default);
                x.For<IShell>().Use<ShellViewModel>();
                x.ForConcreteType<DocumentCollectionViewModel>();
                x.ForConcreteType<DocumentViewModel>();
            });

            // turn off Caliburn Micro conventions
            ViewModelBinder.ApplyConventionsByDefault = false;

            // save original bind action
            var bindAction = ViewModelBinder.Bind;
            ViewModelBinder.Bind = (viewModel, view, context) =>
            {
                // call original bind action
                bindAction(viewModel, view, context);
                var viewFor = view as IViewFor;
                if (viewFor != null)
                {
                    // set the view model of view's that implement the
                    // IViewFor ReactiveUI interface
                    viewFor.ViewModel = viewModel;
                }
            };
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
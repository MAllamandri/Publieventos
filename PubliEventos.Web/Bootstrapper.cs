using System.Web.Mvc;
using Microsoft.Practices.Unity;
using PubliEventos.Contract.Contracts;
using PubliEventos.Services;
using Unity.Mvc4;

namespace PubliEventos.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IServiceLocalities, ServicesLocalitiesHandler>(
                    new HierarchicalLifetimeManager()
                );
            container.RegisterType<IServiceEvents, ServicesEventsHandler>(
                    new HierarchicalLifetimeManager()
                );
            container.RegisterType<IServiceAccounts, ServicesAccountsHandler>(
                    new HierarchicalLifetimeManager()
                );

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}
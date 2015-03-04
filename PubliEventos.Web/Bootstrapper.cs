namespace PubliEventos.Web
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Services;
    using System.Web.Mvc;
    using Unity.Mvc4;

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
            container.RegisterType<ILocalityServices, LocalityServicesHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IEventServices, EventServicesHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IAccountServices, AccountServicesHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommentServices, CommentServicesHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IGroupServices, UsersGroupServicesHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IInvitationServices, InvitationServicesHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportService, ReportServicesHandler>(new HierarchicalLifetimeManager());

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
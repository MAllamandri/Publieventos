namespace PubliEventos.Web.Hubs
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Comment;
    using PubliEventos.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Unity.Mvc4;

    [HubName("CommentHub")]
    public class CommentHub : Hub
    {
        #region Contructor

        /// <summary>
        /// Servicio de comentarios.
        /// </summary>
        private ICommentServices _commentService { get; set; }

        /// <summary>
        /// Constructor del hub.
        /// </summary>
        /// <param name="commentService">Servicio de comentarios.</param>
        public CommentHub(ICommentServices commentService)
        {
            this._commentService = commentService;
        }

        #endregion

        #region Dependency Injection


        public static void Initialise() // this isn't my misspelling, it's in the Unity.MVC NuGet package.  
        {
            var container = BuildUnityContainer();

            var unityDependencyResolver = new UnityDependencyResolver(container);

            // used for MVC
            DependencyResolver.SetResolver(unityDependencyResolver);
            // used for WebAPI
            //GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            // used for SignalR
            GlobalHost.DependencyResolver = new SignalRUnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your dependencies here.
            container.RegisterType<ICommentServices, CommentServicesHandler>();
            container.RegisterType<CommentHub>(new InjectionFactory(CreateMyHub));

            return container;
        }

        private static object CreateMyHub(IUnityContainer p)
        {
            var myHub = new CommentHub(p.Resolve<ICommentServices>());

            return myHub;
        }

        #endregion

        /// <summary>
        /// Agrega un nuevo comentario a todos los usuarios.
        /// </summary>
        /// <param name="detail">Detalle del comentario.</param>
        /// <param name="commentId">Identificador del comentario.</param>
        /// <param name="imageProfile">Imagen de perfil del usuario.</param>
        /// <param name="elapsedTime">Tiempo transcurrido de creacion del comentario.</param>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="userName">Nombre de usuario.</param>
        public void AddNewComment(string detail, int commentId, string imageProfile, string elapsedTime, int userId, string userName)
        {
            Clients.All.addNewCommentToPage(detail, commentId, imageProfile, elapsedTime, userId, userName);
        }

        /// <summary>
        /// Refresca los comentarios a todos los clientes.
        /// </summary>
        public void RefreshComments()
        {
            Clients.All.refreshCommentsInPage();
        }

        /// <summary>
        /// Busca la lista de comentarios de un evento.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>Lista de comentarios.</returns>
        public List<Comment> GetComments(int eventId)
        {
            var comments = this._commentService.GetCommentsByEvent(new GetCommentsByEventRequest() { EventId = eventId }).Comments.OrderByDescending(x => x.EffectDate).ToList();

            return comments;
        }
    }

    public class SignalRUnityDependencyResolver : DefaultDependencyResolver
    {
        private IUnityContainer _container;

        public SignalRUnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            if (_container.IsRegistered(serviceType)) return _container.Resolve(serviceType);
            else return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container.IsRegistered(serviceType)) return _container.ResolveAll(serviceType);
            else return base.GetServices(serviceType);
        }
    }
}

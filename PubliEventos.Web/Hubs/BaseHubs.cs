namespace PubliEventos.Web.Hubs
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Comment;
    using PubliEventos.Services;
    using PubliEventos.Web.App_Start;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Unity.Mvc4;

    [HubName("BaseHubs")]
    public class BaseHubs : Hub
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
        public BaseHubs(ICommentServices commentService)
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
            container.RegisterType<BaseHubs>(new InjectionFactory(CreateMyHub));

            return container;
        }

        private static object CreateMyHub(IUnityContainer p)
        {
            var myHub = new BaseHubs(p.Resolve<ICommentServices>());

            return myHub;
        }

        #endregion

        /// <summary>
        /// Agrega un comentario en todos los usuarios.
        /// </summary>
        /// <param name="commentId">Identificador del comentario.</param>
        public void AddNewComment(int commentId)
        {
            var userId = (System.Web.HttpContext.Current.User as CustomPrincipal).Id;

            var comment = this._commentService.GetCommentById(new GetCommentByIdRequest() { CommentId = commentId, CurrentUserId = userId }).Comment;

            Clients.All.addNewCommentToPage(comment);
        }

        /// <summary>
        /// Refresca los comentarios a todos los clientes.
        /// </summary>
        /// <param name="commentId">Identificador del comentario.</param>
        public void DeleteComment(int commentId)
        {
            Clients.All.DeleteComment(commentId);
        }

        /// <summary>
        /// Edita el comentario a todos los clientes.
        /// </summary>
        /// <param name="commentId">Identificador del comentario.</param>
        /// <param name="detail">Detalle del comentario.</param>
        public void EditComment(int commentId, string detail)
        {
            Clients.All.EditComment(commentId, detail);
        }

        /// <summary>
        /// Busca la lista de comentarios de un evento.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>Lista de comentarios.</returns>
        public List<Comment> GetComments(int eventId)
        {
            var userId = (System.Web.HttpContext.Current.User as CustomPrincipal).Id;

            var comments = this._commentService.GetCommentsByEvent(new GetCommentsByEventRequest() { EventId = eventId, CurrentUserId = userId }).Comments.OrderByDescending(x => x.EffectDate).ToList();

            return comments;
        }

        /// <summary>
        /// Elimina el comentario en todos los usuarios.
        /// </summary>
        /// <param name="contentId">Identificador del contenido.</param>
        /// <param name="contentType">Tipo de contenido.</param>
        public void DeleteContent(string contentId, int contentType)
        {
            Clients.All.DeleteContent(contentId, contentType);
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

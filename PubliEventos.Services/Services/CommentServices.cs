namespace PubliEventos.Services.Services
{
    using NHibernate.Linq;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Services.Comment;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Servicios de comentarios.
    /// </summary>
    public class CommentServices : BaseService
    {
        /// <summary>
        /// Obtiene los comentarios de un evento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static List<Comment> GetCommentsByEvent(GetCommentsByEventRequest request)
        {
            return CurrentSession.Query<Domain.Domain.Comment>().Where(x => x.Event.Id == request.EventId && !x.NullDate.HasValue && x.Active).Select(x => InternalServices.GetCommentSummary(x)).ToList();
        }
    }
}

namespace PubliEventos.Services.Services
{
    using NHibernate.Linq;
    using PubliEventos.Contract.Services.Comment;
    using PubliEventos.DataAccess.Querys;
    using System;
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
        public static GetCommentsByEventResponse GetCommentsByEvent(GetCommentsByEventRequest request)
        {
            var comments = CurrentSession.Query<Domain.Domain.Comment>().Where(x => x.Event.Id == request.EventId && !x.NullDate.HasValue && x.Active).Select(x => InternalServices.GetCommentSummary(x)).ToList();

            return new GetCommentsByEventResponse()
            {
                Comments = comments
            };
        }

        /// <summary>
        /// Crea un comentario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static CreateCommentResponse CreateComment(CreateCommentRequest request)
        {
            var comment = new Domain.Domain.Comment()
            {
                Active = true,
                Detail = request.Detail,
                EffectDate = DateTime.Now,
                Event = CurrentSession.Get<Domain.Domain.Event>(request.EventId),
                User = CurrentSession.Get<Domain.Domain.User>(request.UserId)
            };

            new BaseQuery<Domain.Domain.Comment, int>().Create(comment);

            return new CreateCommentResponse()
            {
                Comment = InternalServices.GetCommentSummary(comment)
            };
        }

        /// <summary>
        /// Edita un comentario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static EditCommentResponse EditComment(EditCommentRequest request)
        {
            var comment = CurrentSession.Get<Domain.Domain.Comment>(request.CommentId);

            comment.Detail = request.Detail;

            new BaseQuery<Domain.Domain.Comment, int>().Update(comment);

            return new EditCommentResponse()
            {
                Comment = InternalServices.GetCommentSummary(comment)
            };
        }
    }
}

namespace PubliEventos.Services
{
    using PubliEventos.Contract.Contracts;
    using Contract.Services.Comment;
    using PubliEventos.Services.Services;

    /// <summary>
    /// Manejador de servicios de comentarios.
    /// </summary>
    public class CommentServicesHandler : ICommentServices
    {
        /// <summary>
        /// Obtiene los comentarios de un evento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public GetCommentsByEventResponse GetCommentsByEvent(GetCommentsByEventRequest request)
        {
            return CommentServices.GetCommentsByEvent(request);
        }

        /// <summary>
        /// Crea un comentario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public CreateCommentResponse CreateComment(CreateCommentRequest request)
        {
            return CommentServices.CreateComment(request);
        }

        /// <summary>
        /// Edita un comentario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public EditCommentResponse EditComment(EditCommentRequest request)
        {
            return CommentServices.EditComment(request);
        }

        /// <summary>
        /// Elimina un comentario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public DeleteCommentResponse DeleteComment(DeleteCommentRequest request)
        {
            return CommentServices.DeleteComment(request);
        }
    }
}

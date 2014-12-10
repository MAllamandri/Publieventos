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
    }
}

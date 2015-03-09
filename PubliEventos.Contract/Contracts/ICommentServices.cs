namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Services.Comment;

    /// <summary>
    /// Interfaz del servicio de comentarios.
    /// </summary>
    public interface ICommentServices
    {
        /// <summary>
        /// Obtiene los comentarios de un evento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        GetCommentsByEventResponse GetCommentsByEvent(GetCommentsByEventRequest request);

        /// <summary>
        /// Crea un comentario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        CreateCommentResponse CreateComment(CreateCommentRequest request);

        /// <summary>
        /// Edita un comentario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        EditCommentResponse EditComment(EditCommentRequest request);

        /// <summary>
        /// Elimina un comentario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        DeleteCommentResponse DeleteComment(DeleteCommentRequest request);

        /// <summary>
        /// Obtiene un comentario por Id.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        GetCommentByIdResponse GetCommentById(GetCommentByIdRequest request);
    }
}

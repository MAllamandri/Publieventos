namespace PubliEventos.Contract.Services.Comment
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación GetCommentById.
    /// </summary>
    public class GetCommentByIdRequest
    {
        /// <summary>
        /// Identificador del comentario.
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// Identificador del usuario logueado.
        /// </summary>
        public int CurrentUserId { get; set; }
    }
}

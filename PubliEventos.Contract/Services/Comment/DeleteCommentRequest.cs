namespace PubliEventos.Contract.Services.Comment
{
    /// <summary>
    /// Parametros del evento DeleteComment.
    /// </summary>
    public class DeleteCommentRequest
    {
        /// <summary>
        /// Identificador del comentario.
        /// </summary>
        public int CommentId { get; set; }
    }
}

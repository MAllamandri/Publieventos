namespace PubliEventos.Contract.Services.Comment
{
    using Contract.Class;

    /// <summary>
    /// Response del servicio EditComment.
    /// </summary>
    public class EditCommentResponse
    {
        /// <summary>
        /// Comentario editado.
        /// </summary>
        public Comment Comment { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Comment
{
    using Contract.Class;

    /// <summary>
    /// Salida de la operación CreateComment.
    /// </summary>
    public class CreateCommentResponse
    {
        /// <summary>
        /// Comentario creado.
        /// </summary>
        public Comment Comment { get; set; }
    }
}

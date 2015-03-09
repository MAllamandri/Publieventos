namespace PubliEventos.Contract.Services.Comment
{
    using Contract.Class;

    /// <summary>
    /// Salidas de la operacíon GetCommentById.
    /// </summary>
    public class GetCommentByIdResponse
    {
        /// <summary>
        /// Comentario.
        /// </summary>
        public Comment Comment { get; set; }
    }
}

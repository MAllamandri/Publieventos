namespace PubliEventos.Contract.Services.Comment
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request del servicio EditComment.
    /// </summary>
    public class EditCommentRequest
    {
        /// <summary>
        /// Identificador del evento.
        /// </summary>
        [Required]
        public int CommentId { get; set; }

        /// <summary>
        /// Detalle del comentario.
        /// </summary>
        [Required]
        public string Detail { get; set; }
    }
}

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
        [Required(ErrorMessage = "El valor es requerido")]
        public int CommentId { get; set; }

        /// <summary>
        /// Detalle del comentario.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public string Detail { get; set; }

        /// <summary>
        /// Identificador del usuario logueado.
        /// </summary>
        public int CurrentUserId { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Comment
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Parametros del evento CreateComment.
    /// </summary>
    public class CreateCommentRequest
    {
        /// <summary>
        /// Identificador del evento.
        /// </summary>
        [Required]
        public int EventId { get; set; }

        /// <summary>
        /// Detalle del comentario.
        /// </summary>
        [Required]
        public string Detail { get; set; }

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }
}

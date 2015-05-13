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
        [Required(ErrorMessage = "El valor es requerido")]
        public int EventId { get; set; }

        /// <summary>
        /// Detalle del comentario.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public string Detail { get; set; }

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public int UserId { get; set; }

        /// <summary>
        /// Fecha local del cliente.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public string Date { get; set; }

        /// <summary>
        /// Hora.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public string TIme { get; set; }
    }
}

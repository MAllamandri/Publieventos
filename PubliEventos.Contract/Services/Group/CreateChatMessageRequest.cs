namespace PubliEventos.Contract.Services.Group
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Representa los parámetros de entrada de la operación CreateChatMessage.
    /// </summary>
    public class CreateChatMessageRequest
    {
        /// <summary>
        /// Identificador del grupo.
        /// </summary>
        [Required]
        public int GroupId { get; set; }

        /// <summary>
        /// Identificador del usuario que hizo el comentario.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Mensaje que se envió.
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        /// Fecha del comentario.
        /// </summary>
        public DateTime EffectDate { get; set; }
    }
}

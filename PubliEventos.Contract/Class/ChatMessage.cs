namespace PubliEventos.Contract.Class
{
    using System;

    /// <summary>
    /// Representa un mensaje de chat.
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Usuario que realizo el mensaje.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Identificador del mensaje.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mensaje.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Identificador del grupo.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Fecha de alta.
        /// </summary>
        public DateTime EffectDate { get; set; }
    }
}

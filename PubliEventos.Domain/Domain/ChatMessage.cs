namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;

    /// <summary>
    /// Representa un mensaje de chat.
    /// </summary>
    public class ChatMessage : BaseIdentifier<int>
    {
        /// <summary>
        /// Grupo al que pertence el mensaje.
        /// </summary>
        public virtual Group Group { get; set; }

        /// <summary>
        /// Usuario que envío el mensaje.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Mensaje.
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Fecha de alta.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }

        /// <summary>
        /// Fecha de baja.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }
    }
}

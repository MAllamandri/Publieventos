namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;

    /// <summary>
    /// Representa un tipo de evento.
    /// </summary>
    public class EventType : BaseIdentifier<int>
    {
        /// <summary>
        /// Description del tipo de evento.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Fecha de alta.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }

        /// <summary>
        /// Indica si esta eliminado.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }
    }
}

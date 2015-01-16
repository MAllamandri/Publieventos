namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;

    /// <summary>
    /// Representa una invitación.
    /// </summary>
    public class Invitation : BaseIdentifier<int>
    {
        /// <summary>
        /// Grupo al que fue invitado.
        /// </summary>
        public virtual Group Group { get; set; }

        /// <summary>
        /// Evento al que fue invitado.
        /// </summary>
        public virtual Event Event { get; set; }

        /// <summary>
        /// Usuario invitado.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Indica si esta confirmado o no.
        /// </summary>
        public virtual bool? Confirmed { get; set; }

        /// <summary>
        /// Fecha de alta.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }

        /// <summary>
        /// Fecha de eliminación.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }
    }
}

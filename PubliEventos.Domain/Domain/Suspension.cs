namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;

    /// <summary>
    /// Representa una suspensión de un usuario.
    /// </summary>
    public class Suspension : BaseIdentifier<int>
    {
        /// <summary>
        /// Usuario suspendido.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Fecha de fin de la suspensión.
        /// </summary>
        public virtual DateTime EndDate { get; set; }

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

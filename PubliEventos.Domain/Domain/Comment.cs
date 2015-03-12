namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Representa un comentario.
    /// </summary>
    public class Comment : BaseIdentifier<int>
    {
        /// <summary>
        /// Evento al que corresponde el comentario.
        /// </summary>
        public virtual Event Event { get; set; }

        /// <summary>
        /// Usuario que realiza el comentario.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Detalle del comentario.
        /// </summary>
        public virtual string Detail { get; set; }

        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }

        /// <summary>
        /// Indica si está activo.
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// Indica si está dado de baja.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }

        /// <summary>
        /// Reportes que recibio el comentario.
        /// </summary>
        public virtual IList<Report> Reports { get; set; }
    }
}

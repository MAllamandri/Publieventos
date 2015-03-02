namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;

    /// <summary>
    /// Representa un reporte.
    /// </summary>
    public class Report : BaseIdentifier<int>
    {
        /// <summary>
        /// Evento reportado.
        /// </summary>
        public virtual Event Event { get; set; }

        /// <summary>
        /// Comentario reportado.
        /// </summary>
        public virtual Comment Comment { get; set; }

        /// <summary>
        /// Contenido reportado.
        /// </summary>
        public virtual MultimediaContent MultimediaContent { get; set; }

        /// <summary>
        /// Usuario que reporto.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Motivo del reporte.
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Indica si el administrador reporto o no el contenido.
        /// </summary>
        public virtual bool? IsReported { get; set; }

        /// <summary>
        /// Fecha de alta.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }

        /// <summary>
        /// Fecha de baja.
        /// </summary>
        public virtual DateTime NullDate { get; set; }
    }
}

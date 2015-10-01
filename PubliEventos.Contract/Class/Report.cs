namespace PubliEventos.Contract.Class
{
    using System;

    /// <summary>
    /// Representa un reporte.
    /// </summary>
    public class Report : BaseClass
    {
        /// <summary>
        /// Evento.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// Comentario.
        /// </summary>
        public Comment Comment { get; set; }

        /// <summary>
        /// Usuario que reporto.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Motivo del reporte.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Indica si el admin aprobo el reporte.
        /// </summary>
        public bool? IsReported { get; set; }

        /// <summary>
        /// Fecha del reporte.
        /// </summary>
        public DateTime EffectDate { get; set; }
    }
}

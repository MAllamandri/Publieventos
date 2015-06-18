namespace PubliEventos.Contract.Services.Report
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Representa los parámetros de entrada de la operación AdministrationReported.
    /// </summary>
    public class AdministrationReportedRequest
    {
        /// <summary>
        /// Identificador del contenido.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public string ContentId { get; set; }

        /// <summary>
        /// Tipo de contenido.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public int ContentType { get; set; }

        /// <summary>
        /// Indica si hay que deshabilitar el contenido.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Identificador del evento.
        /// </summary>
        public int? EventId { get; set; }
    }
}

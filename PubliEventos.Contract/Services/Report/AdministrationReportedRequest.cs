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
        [Required]
        public string ContentId { get; set; }

        /// <summary>
        /// Tipo de contenido.
        /// </summary>
        [Required]
        public int ContentType { get; set; }

        /// <summary>
        /// Indica si hay que deshabilitar el contenido.
        /// </summary>
        [Required]
        public bool IsDisabled { get; set; }
    }
}

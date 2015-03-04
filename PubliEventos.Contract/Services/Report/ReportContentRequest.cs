namespace PubliEventos.Contract.Services.Report
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Representa los parámetros de entrada de la operación ReportContent.
    /// </summary>
    public class ReportContentRequest
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
        /// Usuario que realiza el reporte.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Motivo del reporte.
        /// </summary>
        public string Reason { get; set; }
    }
}

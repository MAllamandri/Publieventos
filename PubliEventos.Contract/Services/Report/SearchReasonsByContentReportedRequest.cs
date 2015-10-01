namespace PubliEventos.Contract.Services.Report
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación SearchReasonsByContentReported.
    /// </summary>
    public class SearchReasonsByContentReportedRequest
    {
        /// <summary>
        /// Identificador del contenido.
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        /// Identificador del tipo de contenido.
        /// </summary>
        public int ContentTypeId { get; set; }

        /// <summary>
        /// Identificador del evento.
        /// </summary>
        public int? EventId { get; set; }
    }
}

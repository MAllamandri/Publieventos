namespace PubliEventos.Contract.Services.Report
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación EvaluateReportsForDisabled.
    /// </summary>
    public class EvaluateReportsForDisabledRequest
    {
        /// <summary>
        /// Identificador del contenido.
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        /// Tipo de contenido.
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>
        /// Evento al que pertence el contenido.
        /// </summary>
        public int EventId { get; set; }
    }
}

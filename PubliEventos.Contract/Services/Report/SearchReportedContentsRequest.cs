namespace PubliEventos.Contract.Services.Report
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación SearchReportedContents.
    /// </summary>
    public class SearchReportedContentsRequest
    {
        /// <summary>
        /// Identificador del usuario actual.
        /// </summary>
        public int CurrentUserId { get; set; }
    }
}

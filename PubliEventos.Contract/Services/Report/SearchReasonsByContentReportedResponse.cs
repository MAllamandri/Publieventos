namespace PubliEventos.Contract.Services.Report
{
    using PubliEventos.Contract.Class;
    using System.Collections.Generic;

    /// <summary>
    /// Salidas de la operación SearchReasonsByContentReported.
    /// </summary>
    public class SearchReasonsByContentReportedResponse
    {
        /// <summary>
        /// Lista de reportes.
        /// </summary>
        public List<Report> Reports { get; set; }
    }
}

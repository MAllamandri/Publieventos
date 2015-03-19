namespace PubliEventos.Services
{
    using Contract.Services.Report;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Services.Services;

    /// <summary>
    /// Manejador de servicios de reportes.
    /// </summary>
    public class ReportServicesHandler : IReportService
    {
        /// <summary>
        /// Reporta un contenido.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public ReportContentResponse ReportContent(ReportContentRequest request)
        {
            return ReportServices.ReportContent(request);
        }

        /// <summary>
        /// Evalua la cantidad de reportes de un contenido, para ver su tratamiento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public EvaluateReportsForDisabledResponse EvaluateReportsForDisabled(EvaluateReportsForDisabledRequest request)
        {
            return ReportServices.EvaluateReportsForDisabled(request);
        }

        /// <summary>
        /// Obtiene los contenidos reportados por los usuarios.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public SearchReportedContentsResponse SearchReportedContents(SearchReportedContentsRequest request)
        {
            return ReportServices.SearchReportedContents(request);
        }
    }
}

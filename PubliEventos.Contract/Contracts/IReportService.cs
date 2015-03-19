namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Services.Report;

    /// <summary>
    /// Servicio de reportes.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Reporta un contenido.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        ReportContentResponse ReportContent(ReportContentRequest request);

        /// <summary>
        /// Evalua la cantidad de reportes de un contenido, para ver su tratamiento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        EvaluateReportsForDisabledResponse EvaluateReportsForDisabled(EvaluateReportsForDisabledRequest request);

        /// <summary>
        /// Obtiene los contenidos reportados por los usuarios.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        SearchReportedContentsResponse SearchReportedContents(SearchReportedContentsRequest request);
    }
}

﻿namespace PubliEventos.Contract.Contracts
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
    }
}
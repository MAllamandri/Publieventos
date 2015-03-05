namespace PubliEventos.Web.Models.EventModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Modelo detalle de conteidos multimedia.
    /// </summary>
    public class MultimediaContentSummaryModel
    {
        /// <summary>
        /// Identificador del archivo (único).
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Reportes que recibio el contenido.
        /// </summary>
        public bool IsReportedByUser { get; set; }
    }
}
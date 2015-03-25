namespace PubliEventos.Contract.Services.Report
{
    using Contract.Class;
    using System.Collections.Generic;

    /// <summary>
    /// Salidas de la operación SearchReportedContents.
    /// </summary>
    public class SearchReportedContentsResponse
    {
        /// <summary>
        /// Eventos reportados.
        /// </summary>
        public List<Event> Events { get; set; }

        /// <summary>
        /// Comentarios reportados.
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// Fotos reportados.
        /// </summary>
        public List<MultimediaContent> Pictures { get; set; }

        /// <summary>
        /// Videos reportados.
        /// </summary>
        public List<MultimediaContent> Movies { get; set; }
    }
}

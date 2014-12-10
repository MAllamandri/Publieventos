namespace PubliEventos.Contract.Services.Event
{
    using System;

    /// <summary>
    /// Parametros para la busqueda de eventos por diferentes filtros.
    /// </summary>
    public class SearchFilteredEventsRequest
    {
        /// <summary>
        /// Identificador del tipo de evento.
        /// </summary>
        public int? EventTypeId { get; set; }

        /// <summary>
        /// Fecha desde para búsqueda.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Fecha hasta para búsqueda.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Indica si debo fltrar mis eventos o todos.
        /// </summary>
        public bool MyEvents { get; set; }

        /// <summary>
        /// Identificador del usuario para filtrar eventos.
        /// </summary>
        public int? IdUser { get; set; }
    }
}

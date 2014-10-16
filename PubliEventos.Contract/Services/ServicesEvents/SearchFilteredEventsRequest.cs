namespace PubliEventos.Contract.Services.ServicesEvents
{
    using System;

    /// <summary>
    /// Parametros para la busqueda de eventos por diferentes filtros.
    /// </summary>
    public class SearchFilteredEventsRequest
    {
        /// <summary>
        /// Identificador de la localidad.
        /// </summary>
        public int? LocalityId { get; set; }

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
    }
}

namespace PubliEventos.Contract.Services.Event
{
    using PubliEventos.Contract.Class;
    using System.Collections.Generic;

    /// <summary>
    /// Representa la salida de la operación SearchFilteredEvents.
    /// </summary>
    public class SearchFilteredEventsResponse
    {
        /// <summary>
        /// Lista de eventos filtrados.
        /// </summary>
        public List<Event> Events { get; set; }
    }
}

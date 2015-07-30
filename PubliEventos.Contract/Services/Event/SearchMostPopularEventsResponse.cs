namespace PubliEventos.Contract.Services.Event
{
    using System.Collections.Generic;
    using PubliEventos.Contract.Class;

    /// <summary>
    /// Salidas de la operación SearchMostPopularEvents.
    /// </summary>
    public class SearchMostPopularEventsResponse
    {
        public List<Event> Events { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Event
{
    using System.Collections.Generic;
    using Contract.Class;

    /// <summary>
    /// Salidas de la operación SearchEventsByDistance.
    /// </summary>
    public class SearchEventsByDistanceResponse
    {
        public List<Event> Events { get; set; }
    }
}

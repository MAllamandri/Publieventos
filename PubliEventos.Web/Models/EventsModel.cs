namespace PubliEventos.Web.Models
{
    using System.Collections.Generic;
    using PubliEventos.Contract.Class;

    /// <summary>
    /// Modelo de eventos.
    /// </summary>
    public class EventsModel
    {
        public List<Event> events { get; set; }
    }
}
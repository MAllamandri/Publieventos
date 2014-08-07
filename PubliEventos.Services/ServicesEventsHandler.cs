namespace PubliEventos.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Services.Services;

    /// <summary>
    /// Manejador de eventos.
    /// </summary>
    public class ServicesEventsHandler : IServiceEvents
    {
        /// <summary>
        /// Obtengo todas los eventos.
        /// </summary>
        /// <returns></returns>
        public List<Domain.Domain.Event> GetAllEvents()
        {
            return ServiceEvents.GatAllEvents();
        }


        public List<Domain.Domain.MultimediaContent> All()
        {
            return ServiceEvents.All();
        }
    }
}

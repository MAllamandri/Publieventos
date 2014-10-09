namespace PubliEventos.Services
{
    using System.Collections.Generic;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.ServicesEvents;
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
        public List<Event> GetAllEvents()
        {
            return ServiceEvents.GatAllEvents();
        }

        /// <summary>
        /// Obtiene todos los tipos de eventos.
        /// </summary>
        /// <returns>Lista de tipos de evetnos.</returns>
        public List<EventType> GetAllEventTypes()
        {
            return ServiceEvents.GetAllEventTypes();
        }

        /// <summary>
        /// Guarda un evento.
        /// </summary>
        /// <param name="parameters">Parametros de entrada.</param>
        public void CreateEvent(EventCreateOrUpdateParameters parameters)
        {
            ServiceEvents.CreateEvent(parameters);
        }

        /// <summary>
        /// Recupera un evento por su id.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Evento.</returns>
        public Event GetEventById(int id)
        {
            return ServiceEvents.GetEventById(id);
        }

        /// <summary>
        /// Edita un evento.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        public void EditEvent(EventCreateOrUpdateParameters request)
        {
            ServiceEvents.EditEvent(request);
        }

        /// <summary>
        /// Elimina un evento.
        /// </summary>
        /// <param name="idEvent">Identificador del evento.</param>
        public void DeleteEvent(int idEvent)
        {
            ServiceEvents.DeleteEvent(idEvent);
        }
    }
}

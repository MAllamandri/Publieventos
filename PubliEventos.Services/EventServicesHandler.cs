namespace PubliEventos.Services
{
    using System.Collections.Generic;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Event;
    using PubliEventos.Services.Services;

    /// <summary>
    /// Manejador de eventos.
    /// </summary>
    public class EventServicesHandler : IEventServices
    {
        /// <summary>
        /// Obtiene todos los tipos de eventos.
        /// </summary>
        /// <returns>Lista de tipos de evetnos.</returns>
        public List<EventType> GetAllEventTypes()
        {
            return EventServices.GetAllEventTypes();
        }

        /// <summary>
        /// Guarda un evento.
        /// </summary>
        /// <param name="parameters">Parametros de entrada.</param>
        public EventCreateOrUpdateResponse CreateEvent(EventCreateOrUpdateRequest parameters)
        {
            return EventServices.CreateEvent(parameters);
        }

        /// <summary>
        /// Recupera un evento por su id.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Evento.</returns>
        public Event GetEventById(int id)
        {
            return EventServices.GetEventById(id);
        }

        /// <summary>
        /// Edita un evento.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        public void EditEvent(EventCreateOrUpdateRequest request)
        {
            EventServices.EditEvent(request);
        }

        /// <summary>
        /// Elimina un evento.
        /// </summary>
        /// <param name="idEvent">Identificador del evento.</param>
        public void DeleteEvent(int idEvent)
        {
            EventServices.DeleteEvent(idEvent);
        }

        /// <summary>
        /// Obtiene eventos por diferentes filtros.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>Lista de eventos filtrados.</returns>
        public SearchFilteredEventsResponse SearchFilteredEvents(SearchFilteredEventsRequest request)
        {
            return EventServices.SearchFilteredEvents(request);
        }

        /// <summary>
        /// Da de alta un contenido multimedia asociado al evento.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>Lista de eventos filtrados.</returns>
        public CreateMultimediaContentResponse CreateMultimediaContent(CreateMultimediaContentRequest request)
        {
            return EventServices.CreateMultimediaContent(request);
        }

        /// <summary>
        /// Da de baja un contenido multimedia asociado al evento.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>Lista de eventos filtrados.</returns>
        public DeleteMultimediaContentResponse DeleteMultimediaContent(DeleteMultimediaContentRequest request)
        {
            return EventServices.DeleteMultimediaContent(request);
        }

        /// <summary>
        /// Busca eventos por aproximación.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>Lista de eventos filtrados.</returns>
        public SearchEventsByDistanceResponse SearchEventsByDistance(SearchEventsByDistanceRequest request)
        {
            return EventServices.SearchEventsByDistance(request);
        }

        /// <summary>
        /// Indica si el contenido ya existe.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>Lista de eventos filtrados.</returns>
        public ValidateExistsContentResponse ValidateExistsContent(ValidateExistsContentRequest request)
        {
            return EventServices.ValidateExistsContent(request);
        }
    }
}

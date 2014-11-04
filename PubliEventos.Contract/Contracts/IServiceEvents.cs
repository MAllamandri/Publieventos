namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Services.ServicesEvents;
    using System.Collections.Generic;

    /// <summary>
    /// Interface del servicio de eventos.
    /// </summary>
    public interface IServiceEvents
    {
        /// <summary>
        /// Obtengo todos los usuarios.
        /// </summary>
        /// <returns>Lista de usuarios.</returns>
        List<Event> GetAllEvents();

        /// <summary>
        /// Obtiene todos los tipos de eventos.
        /// </summary>
        /// <returns>Lista de tipos de evetnos.</returns>
        List<EventType> GetAllEventTypes();

        /// <summary>
        /// Guarda un evento.
        /// </summary>
        /// <param name="parameters">Parametros de entrada.</param>
        void CreateEvent(EventCreateOrUpdateRequest request);

        /// <summary>
        /// Recupera un evento por su id.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Evento.</returns>
        Event GetEventById(int id);

        /// <summary>
        /// Edita un evento.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        void EditEvent(EventCreateOrUpdateRequest request);

        /// <summary>
        /// Elimina un evento.
        /// </summary>
        /// <param name="idEvent">Identificador del evento.</param>
        void DeleteEvent(int idEvent);

        /// <summary>
        /// Obtiene eventos por diferentes filtros.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>Lista de eventos filtrados.</returns>
        List<Event> SearchFilteredEvents(SearchFilteredEventsRequest request);
    }
}

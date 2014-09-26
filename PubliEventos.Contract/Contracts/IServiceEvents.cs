namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Services.ServicesEvents;
    using System.Collections.Generic;
    using PubliEventos.Contract.Class;

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
        void CreateEvent(EventCreateOrUpdateParameters request);

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
        void EditEvent(EventCreateOrUpdateParameters request);
    }
}

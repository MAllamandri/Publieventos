﻿namespace PubliEventos.Services
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
        /// Obtengo todas los eventos.
        /// </summary>
        /// <returns></returns>
        public List<Event> GetAllEvents()
        {
            return EventServices.GatAllEvents();
        }

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
        public void CreateEvent(EventCreateOrUpdateRequest parameters)
        {
            EventServices.CreateEvent(parameters);
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
        public List<Event> SearchFilteredEvents(SearchFilteredEventsRequest request)
        {
            return EventServices.SearchFilteredEvents(request);
        }
    }
}
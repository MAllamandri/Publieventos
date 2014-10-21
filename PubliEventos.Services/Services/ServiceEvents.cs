namespace PubliEventos.Services.Services
{
    using LinqKit;
    using NHibernate.Linq;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Services.ServicesEvents;
    using PubliEventos.DataAccess.Querys;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    /// <summary>
    /// Servicio de eventos.
    /// </summary>
    public class ServiceEvents : BaseService
    {
        /// <summary>
        /// Obtiene todos los eventos.
        /// </summary>
        /// <returns>Lista de eventos.</returns>
        public static List<Event> GatAllEvents()
        {
            return new BaseQuery<Domain.Domain.Event, int>().LoadAll().Where(x => !x.NullDate.HasValue && x.Active).Select(x => GetEventSummary(x)).ToList();
        }

        /// <summary>
        /// Obtiene todos los tipos de eventos.
        /// </summary>
        /// <returns>Lista de tipos de evetnos.</returns>
        public static List<EventType> GetAllEventTypes()
        {
            return
                new BaseQuery<Domain.Domain.EventType, int>().LoadAll()
                    .Where(x => !x.NullDate.HasValue)
                    .Select(x => new EventType()
                    {
                        Id = x.Id,
                        Description = x.Description
                    }).ToList();
        }

        /// <summary>
        /// Guarda un evento.
        /// </summary>
        /// <param name="request">Parametros de entrada.</param>
        public static void CreateEvent(EventCreateOrUpdateRequest request)
        {
            var eventToSave = new Domain.Domain.Event()
            {
                Active = true,
                Title = request.Title,
                Detail = request.Detail,
                Description = request.Description,
                CreationDate = DateTime.Now,
                User = CurrentSession.Get<Domain.Domain.User>(request.UserId),
                // Locality = CurrentSession.Get<Domain.Domain.Locality>(request.LocalityId),
                EventType = CurrentSession.Get<Domain.Domain.EventType>(request.EventTypeId),
                EventDate = request.EventDate,
                Private = request.Private,
                EventEndTime = request.EventEndTime,
                EventStartTime = request.EventStartTime,
                FileName = !string.IsNullOrEmpty(request.FileName) ? request.FileName : null,
                Latitude = decimal.Parse(request.Latitude, System.Globalization.CultureInfo.InvariantCulture),
                Longitude = decimal.Parse(request.Longitude, System.Globalization.CultureInfo.InvariantCulture)
            };

            new BaseQuery<Domain.Domain.Event, int>().Create(eventToSave);
        }

        /// <summary>
        /// Recupera un evento por su id.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Evento.</returns>
        public static Event GetEventById(int id)
        {
            return CurrentSession.Query<Domain.Domain.Event>().Where(x => x.Id == id && !x.NullDate.HasValue && x.Active).Select(x => GetEventSummary(x)).SingleOrDefault();
        }

        /// <summary>
        /// Edita un evento.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        public static void EditEvent(EventCreateOrUpdateRequest request)
        {
            var eventToSave = CurrentSession.Query<Domain.Domain.Event>().Where(x => x.Id == request.Id).SingleOrDefault();

            eventToSave.Title = request.Title;
            eventToSave.Detail = request.Detail;
            eventToSave.Description = request.Description;
            eventToSave.User = CurrentSession.Get<Domain.Domain.User>(request.UserId);
            // eventToSave.Locality = CurrentSession.Get<Domain.Domain.Locality>(request.LocalityId);
            eventToSave.EventType = CurrentSession.Get<Domain.Domain.EventType>(request.EventTypeId);
            eventToSave.EventDate = request.EventDate;
            eventToSave.Private = request.Private;
            eventToSave.EventEndTime = request.EventEndTime;
            eventToSave.EventStartTime = request.EventStartTime;
            eventToSave.FileName = !string.IsNullOrEmpty(request.FileName) ? request.FileName : null;
            eventToSave.Latitude = decimal.Parse(request.Latitude, System.Globalization.CultureInfo.InvariantCulture);
            eventToSave.Longitude = decimal.Parse(request.Longitude, System.Globalization.CultureInfo.InvariantCulture);

            new BaseQuery<Domain.Domain.Event, int>().Update(eventToSave);
        }

        /// <summary>
        /// Elimina un evento.
        /// </summary>
        /// <param name="idEvent">Identificador del evento.</param>
        public static void DeleteEvent(int idEvent)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var eventToDelete = CurrentSession.Query<Domain.Domain.Event>().Where(x => x.Id == idEvent && !x.NullDate.HasValue && x.Active).Single();

                // Doy de baja el evento.
                eventToDelete.NullDate = DateTime.Now;

                // Doy de baja los comentarios del evento.
                foreach (var comment in eventToDelete.Comments)
                {
                    comment.NullDate = DateTime.Now;
                }

                // Doy de baja los contenidos multimedia que posea el evento.
                foreach (var content in eventToDelete.MultimediaContents)
                {
                    content.NullDate = DateTime.Now;
                }

                transaction.Complete();
            }
        }

        /// <summary>
        /// Obtiene eventos por diferentes filtros.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>Lista de eventos filtrados.</returns>
        public static List<Event> SearchFilteredEvents(SearchFilteredEventsRequest request)
        {
            var predicate = PredicateBuilder.True<Domain.Domain.Event>();
            predicate = predicate.And(x => !x.NullDate.HasValue && x.Active);

            if (request.LocalityId.HasValue)
            {
                predicate = predicate.And(x => x.Locality.Id == request.LocalityId.Value);
            }

            if (request.EventTypeId.HasValue)
            {
                predicate = predicate.And(x => x.EventType.Id == request.EventTypeId.Value);
            }

            if (request.EndDate.HasValue && request.StartDate.HasValue)
            {
                predicate = predicate.And(x => x.EventDate.Date > request.StartDate.Value.Date && x.EventDate.Date <= request.EndDate.Value.Date);
            }
            else if (request.StartDate.HasValue)
            {
                predicate = predicate.And(x => x.EventDate.Date >= request.StartDate.Value.Date);
            }
            else if (request.EndDate.HasValue)
            {
                predicate = predicate.And(x => x.EventDate.Date <= request.EndDate.Value.Date);
            }

            return CurrentSession.Query<Domain.Domain.Event>().Where(predicate).Select(x => GetEventSummary(x)).ToList();
        }

        #region Private Methods

        /// <summary>
        /// Parsea un evento de dominio a un evento de contrato.
        /// </summary>
        /// <param name="eventToParse">Evento a parsear.</param>
        /// <returns>Evento de contrato.</returns>
        private static Event GetEventSummary(Domain.Domain.Event eventToParse)
        {
            return new Event()
            {
                Id = eventToParse.Id,
                Title = eventToParse.Title,
                Detail = eventToParse.Detail,
                Active = eventToParse.Active,
                FileName = eventToParse.FileName,
                Description = eventToParse.Description,
                EventDate = eventToParse.EventDate.Date,
                EventStartTime = eventToParse.EventStartTime,
                EventEndTime = eventToParse.EventEndTime,
                //Locality = new Locality()
                //         {
                //             Id = eventToParse.Locality.Id,
                //             Name = eventToParse.Locality.Name,
                //             Latitude = eventToParse.Locality.Latitude,
                //             Longitude = eventToParse.Locality.Longitude,
                //             Province = new Province()
                //             {
                //                 Id = eventToParse.Locality.Province.Id,
                //                 Name = eventToParse.Locality.Province.Name
                //             }
                //         },
                Private = eventToParse.Private,
                User = new User()
                         {
                             Id = eventToParse.User.Id
                         },
                EventType = new EventType()
                        {
                            Id = eventToParse.EventType.Id,
                            Description = eventToParse.EventType.Description
                        },
                Latitude = eventToParse.Latitude.ToString(),
                Longitude = eventToParse.Longitude.ToString()
            };
        }

        #endregion
    }
}

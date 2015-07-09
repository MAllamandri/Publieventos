namespace PubliEventos.Services.Services
{
    using LinqKit;
    using NHibernate.Linq;
    using NHibernate.Transform;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Services.Event;
    using PubliEventos.DataAccess.Querys;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    /// <summary>
    /// Servicio de eventos.
    /// </summary>
    public class EventServices : BaseService
    {
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
        public static EventCreateOrUpdateResponse CreateEvent(EventCreateOrUpdateRequest request)
        {
            var eventToSave = new Domain.Domain.Event()
            {
                Active = true,
                Title = request.Title,
                Detail = request.Detail,
                Description = request.Description,
                CreationDate = DateTime.Now,
                User = CurrentSession.Get<Domain.Domain.User>(request.UserId),
                EventType = CurrentSession.Get<Domain.Domain.EventType>(request.EventTypeId),
                EventDate = request.EventDate,
                Private = request.Private,
                EventEndTime = request.EventEndTime,
                EventStartTime = request.EventStartTime,
                FileName = !string.IsNullOrEmpty(request.FileName) ? request.FileName : null,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            new BaseQuery<Domain.Domain.Event, int>().Create(eventToSave);

            return new EventCreateOrUpdateResponse
            {
                EventId = eventToSave.Id
            };
        }

        /// <summary>
        /// Recupera un evento por su id.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>Evento.</returns>
        public static Event GetEventById(int id)
        {
            var _event = CurrentSession.Query<Domain.Domain.Event>().Where(x => x.Id == id && !x.NullDate.HasValue && x.Active).SingleOrDefault();

            if (_event == null)
            {
                throw new Exception("El evento no existe o fue dado de baja");
            }

            return InternalServices.GetEventSummary(_event);
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
            eventToSave.EventType = CurrentSession.Get<Domain.Domain.EventType>(request.EventTypeId);
            eventToSave.EventDate = request.EventDate;
            eventToSave.Private = request.Private;
            eventToSave.EventEndTime = request.EventEndTime;
            eventToSave.EventStartTime = request.EventStartTime;
            eventToSave.FileName = !string.IsNullOrEmpty(request.FileName) ? request.FileName : null;
            eventToSave.Latitude = request.Latitude;
            eventToSave.Longitude = request.Longitude;

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
        /// <returns>El resultado de la operación.</returns>
        public static SearchFilteredEventsResponse SearchFilteredEvents(SearchFilteredEventsRequest request)
        {
            var predicate = PredicateBuilder.True<Domain.Domain.Event>();
            predicate = predicate.And(x => !x.NullDate.HasValue && x.Active);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                predicate = predicate.And(x => x.Title.Contains(request.SearchTerm.Trim()) ||
                                               x.Description.Contains(request.SearchTerm.Trim()) ||
                                               x.Detail.Contains(request.SearchTerm.Trim()));
            }

            if (request.SearchPublics)
            {
                predicate = predicate.And(x => x.Private == false);
            }

            if (request.SearchPrivate)
            {
                predicate = predicate.And(x => x.Private == true);
            }

            if (request.UserId.HasValue)
            {
                predicate = predicate.And(x => x.User.Id == request.UserId.Value);
            }

            if (request.EventTypeId.HasValue)
            {
                predicate = predicate.And(x => x.EventType.Id == request.EventTypeId.Value);
            }

            if (request.EndDate.HasValue && request.StartDate.HasValue)
            {
                predicate = predicate.And(x => x.EventDate.Date >= request.StartDate.Value.Date && x.EventDate.Date <= request.EndDate.Value.Date);
            }
            else if (request.StartDate.HasValue)
            {
                predicate = predicate.And(x => x.EventDate.Date >= request.StartDate.Value.Date);
            }
            else if (request.EndDate.HasValue)
            {
                predicate = predicate.And(x => x.EventDate.Date <= request.EndDate.Value.Date);
            }

            var events = CurrentSession.Query<Domain.Domain.Event>()
                            .Where(predicate)
                            .Select(x => InternalServices.GetEventSummary(x))
                            .Take(500)
                            .ToList();

            return new SearchFilteredEventsResponse()
            {
                Events = events
            };
        }

        /// <summary>
        /// Da de alta un contenido multimedia asociado al evento.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static CreateMultimediaContentResponse CreateMultimediaContent(CreateMultimediaContentRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var _event = CurrentSession.Get<Domain.Domain.Event>(request.EventId);

                if (_event.MultimediaContents == null)
                {
                    _event.MultimediaContents = new List<Domain.Domain.MultimediaContent>();
                }

                var content = new Domain.Domain.MultimediaContent()
                {
                    Active = true,
                    ContentType = request.ContentType,
                    EffectDate = DateTime.Now,
                    Event = _event,
                    Name = request.FileName
                };

                new BaseQuery<Domain.Domain.MultimediaContent, int>().Create(content);

                transaction.Complete();

                return new CreateMultimediaContentResponse();
            }
        }

        /// <summary>
        /// Da de baja un contenido multimedia asociado al evento.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static DeleteMultimediaContentResponse DeleteMultimediaContent(DeleteMultimediaContentRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var _event = CurrentSession.Get<Domain.Domain.Event>(request.EventId);

                if (_event.MultimediaContents.Where(x => x.Name == request.FileName && !x.NullDate.HasValue).Any())
                {
                    foreach (var content in _event.MultimediaContents.Where(x => x.Name == request.FileName && !x.NullDate.HasValue))
                    {
                        content.NullDate = DateTime.Now;
                    }
                }

                transaction.Complete();

                return new DeleteMultimediaContentResponse();
            }
        }

        /// <summary>
        /// Busca eventos por aproximación.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>Lista de eventos filtrados.</returns>
        public static SearchEventsByDistanceResponse SearchEventsByDistance(SearchEventsByDistanceRequest request)
        {

            var eventsIds = CurrentSession.GetNamedQuery("SearchEventsByDistance")
                                      .SetString("LatitudeInitial", request.LatitudeInitial)
                                      .SetString("LongitudeInitial", request.LongitudeInitial)
                                      .SetParameter<int>("MaxDistance", request.MaxDistance)
                                      .SetResultTransformer(Transformers.AliasToBean<SearchEventsByDistanceResult>())
                                      .List<SearchEventsByDistanceResult>().Select(x => x.Id).ToList();

            var events = CurrentSession.Query<Domain.Domain.Event>().Where(x => eventsIds.Any() && eventsIds.Contains(x.Id))
                                                                    .Select(x => new Event
                                                                    {
                                                                        Id = x.Id,
                                                                        Title = x.Title,
                                                                        Latitude = x.Latitude,
                                                                        Longitude = x.Longitude
                                                                    }).ToList();

            return new SearchEventsByDistanceResponse()
            {
                Events = events
            };
        }

        /// <summary>
        /// Indica si el contenido ya existe.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        /// <returns>Lista de eventos filtrados.</returns>
        public static ValidateExistsContentResponse ValidateExistsContent(ValidateExistsContentRequest request)
        {
            var _event = CurrentSession.Get<Domain.Domain.Event>(request.EventId);

            var exists = false;

            if (_event.MultimediaContents.Any() && _event.MultimediaContents.Where(x => x.Name == request.FileName &&
                                                                                        x.ContentType == request.ContentType &&
                                                                                        !x.NullDate.HasValue).Any())
            {
                exists = true;
            }

            return new ValidateExistsContentResponse
            {
                Exists = exists
            };
        }
    }
}

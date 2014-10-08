namespace PubliEventos.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate.Linq;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Services.ServicesEvents;
    using PubliEventos.DataAccess.Querys;

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
            return new BaseQuery<Domain.Domain.Event, int>().LoadAll().Where(x => !x.NullDate.HasValue && x.Active).Select(x => new Event()
            {
                Id = x.Id,
                Title = x.Title,
                Detail = x.Detail,
                Active = x.Active,
                FileName = x.FileName,
                CreationDate = x.CreationDate,
                Description = x.Description,
                EventDate = x.EventDate.Date,
                EventStartTime = x.EventStartTime,
                EventEndTime = x.EventEndTime,
                Locality = new Locality()
                {
                    Id = x.Locality.Id,
                    Name = x.Locality.Name,
                    Latitude = x.Locality.Latitude,
                    Longitude = x.Locality.Longitude,
                    Province = new Province()
                    {
                        Id = x.Locality.Province.Id,
                        Name = x.Locality.Province.Name
                    }
                },
                Private = x.Private,
                User = new User()
                {
                    Id = x.User.Id
                },
                EventType = new EventType()
                {
                    Id = x.EventType.Id,
                    Description = x.EventType.Description
                }
            }).ToList();
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
        public static void CreateEvent(EventCreateOrUpdateParameters request)
        {
            var eventToSave = new Domain.Domain.Event()
            {
                Active = true,
                Title = request.Title,
                Detail = request.Detail,
                Description = request.Description,
                CreationDate = DateTime.Now,
                User = CurrentSession.Get<Domain.Domain.User>(request.UserId),
                Locality = CurrentSession.Get<Domain.Domain.Locality>(request.LocalityId),
                EventType = CurrentSession.Get<Domain.Domain.EventType>(request.EventTypeId),
                EventDate = request.EventDate,
                Private = request.Private,
                EventEndTime = request.EventEndTime,
                EventStartTime = request.EventStartTime,
                FileName = !string.IsNullOrEmpty(request.FileName) ? request.FileName : null
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
            return
                CurrentSession.Query<Domain.Domain.Event>().Where(x => x.Id == id && !x.NullDate.HasValue && x.Active).Select(x => new Event()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Detail = x.Detail,
                        Active = x.Active,
                        FileName = x.FileName,
                        CreationDate = x.CreationDate,
                        Description = x.Description,
                        EventDate = x.EventDate,
                        EventStartTime = x.EventStartTime,
                        EventEndTime = x.EventEndTime,
                        Locality = new Locality()
                        {
                            Id = x.Locality.Id,
                            Name = x.Locality.Name,
                            Latitude = x.Locality.Latitude,
                            Longitude = x.Locality.Longitude,
                            Province = new Province()
                            {
                                Id = x.Locality.Province.Id,
                                Name = x.Locality.Province.Name
                            }
                        },
                        Private = x.Private,
                        User = new User()
                        {
                            Id = x.User.Id
                        },
                        EventType = new EventType()
                        {
                            Id = x.EventType.Id,
                            Description = x.EventType.Description
                        }
                    }).SingleOrDefault();
        }

        /// <summary>
        /// Edita un evento.
        /// </summary>
        /// <param name="request">Parámetros de entrada.</param>
        public static void EditEvent(EventCreateOrUpdateParameters request)
        {
            var eventToSave = CurrentSession.Query<Domain.Domain.Event>().Where(x => x.Id == request.Id).SingleOrDefault();

            eventToSave.Title = request.Title;
            eventToSave.Detail = request.Detail;
            eventToSave.Description = request.Description;
            eventToSave.User = CurrentSession.Get<Domain.Domain.User>(request.UserId);
            eventToSave.Locality = CurrentSession.Get<Domain.Domain.Locality>(request.LocalityId);
            eventToSave.EventType = CurrentSession.Get<Domain.Domain.EventType>(request.EventTypeId);
            eventToSave.EventDate = request.EventDate;
            eventToSave.Private = request.Private;
            eventToSave.EventEndTime = request.EventEndTime;
            eventToSave.EventStartTime = request.EventStartTime;
            eventToSave.FileName = !string.IsNullOrEmpty(request.FileName) ? request.FileName : null;

            new BaseQuery<Domain.Domain.Event, int>().Update(eventToSave);
        }
    }
}

namespace PubliEventos.Services.Services
{
    using PubliEventos.Contract.Class;
    using System.Linq;

    public class InternalServices
    {
        /// <summary>
        /// Parsea un evento de dominio a un evento de contrato.
        /// </summary>
        /// <param name="eventToParse">Evento a parsear.</param>
        /// <returns>Evento de contrato.</returns>
        public Event GetEventSummary(Domain.Domain.Event eventToParse)
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
                Private = eventToParse.Private,
                User = GetUserSummary(eventToParse.User),
                EventType = new EventType()
                {
                    Id = eventToParse.EventType.Id,
                    Description = eventToParse.EventType.Description
                },
                Latitude = eventToParse.Latitude,
                Longitude = eventToParse.Longitude
            };
        }

        /// <summary>
        /// Pasea un comentario.
        /// </summary>
        /// <param name="comment">Comentario.</param>
        /// <returnsComentario de contrato.></returns>
        public Comment GetCommentSummary(Domain.Domain.Comment comment)
        {
            return new Comment()
            {
                Detail = comment.Detail,
                Active = comment.Active,
                EffectDate = comment.EffectDate,
                Id = comment.Id,
                NullDate = comment.NullDate,
                Event = GetEventSummary(comment.Event),
                User = GetUserSummary(comment.User),
                ElapsedTime = comment.ElapsedTime
            };
        }

        /// <summary>
        /// Parsea un usuario.
        /// </summary>
        /// <param name="user">Usuario.</param>
        /// <returns>Usuario de contrato.</returns>
        public User GetUserSummary(Domain.Domain.User user)
        {
            return new User()
            {
                Active = user.Active,
                BirthDate = user.BirthDate,
                EffectDate = user.EffectDate,
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                ImageProfile = user.ImageProfile,
                LastName = user.LastName,
                NullDate = user.NullDate,
                Password = user.Password,
                UserName = user.UserName
            };
        }

        /// <summary>
        /// Parsea un grupo.
        /// </summary>
        /// <param name="group">Grupo.</param>
        /// <returns>Grupo de contrato.</returns>
        public Group GetGroupSummary(Domain.Domain.Group group)
        {
            return new Group()
            {
                Administrator = this.GetUserSummary(group.Administrator),
                Id = group.Id,
                Name = group.Name,
                Message = group.Message,
                Users = group.Users.Select(x => this.GetUserSummary(x)).ToList()
            };
        }
    }
}

namespace PubliEventos.Services.Services
{
    using PubliEventos.Contract.Class;
    using System.Collections.Generic;
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
            if (eventToParse == null)
            {
                return null;
            }

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
                Longitude = eventToParse.Longitude,
                MultimediaContents = eventToParse.MultimediaContents.Any() ? eventToParse.MultimediaContents.Select(x => GetMultimediaContentSummary(x)).ToList() : null,
                Reports = eventToParse.Reports.Any() ? eventToParse.Reports.Select(x => GetReportSummary(x)).ToList() : null
            };
        }

        /// <summary>
        /// Pasea un comentario.
        /// </summary>
        /// <param name="comment">Comentario.</param>
        /// <returnsComentario de contrato.></returns>
        public Comment GetCommentSummary(Domain.Domain.Comment comment, int currentUserId)
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
                IsReportedByUser = comment.Reports != null && comment.Reports.Any() && comment.Reports.Select(x => x.User.Id).ToList().Contains(currentUserId) ? true : false
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
                UserName = user.UserName,
                Locality = this.GetLocalitySummary(user.Locality)
            };
        }

        /// <summary>
        /// Parsea una localidad.
        /// </summary>
        /// <param name="locality">Localidad.</param>
        /// <returns>Localidad.</returns>
        public Locality GetLocalitySummary(Domain.Domain.Locality locality)
        {
            return new Locality()
            {
                Id = locality.Id,
                Name = locality.Name,
                Latitude = locality.Latitude,
                Longitude = locality.Longitude,
                Province = new Province()
                {
                    Id = locality.Province.Id,
                    Name = locality.Province.Name
                }
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
                Users = group.Users.Select(x => this.GetUserSummary(x)).ToList(),
                UsersGroup = group.UsersGroup.Select(x => new UserGroup()
                {
                    GroupId = x.GroupId,
                    UserId = x.UserId,
                    Active = x.Active
                }).ToList()
            };
        }

        /// <summary>
        /// Parsea una invitación.
        /// </summary>
        /// <param name="invitation">Invitación.</param>
        /// <returns>Retorna una invitación de contrato.</returns>
        public Invitation GetInvitationSummary(Domain.Domain.Invitation invitation)
        {
            return new Invitation()
            {
                Id = invitation.Id,
                Confirmed = invitation.Confirmed,
                Event = invitation.Event != null ? this.GetEventSummary(invitation.Event) : null,
                Group = invitation.Group != null ? this.GetGroupSummary(invitation.Group) : null,
                User = this.GetUserSummary(invitation.User)
            };
        }

        /// <summary>
        /// Parsea un reporte.
        /// </summary>
        /// <param name="report">Reporte.</param>
        /// <returns>Reporte de contrato.</returns>
        public Report GetReportSummary(Domain.Domain.Report report)
        {
            return new Report()
            {
                Id = report.Id,
                Reason = report.Reason,
                User = GetUserSummary(report.User)
            };
        }

        /// <summary>
        /// Parsea un contenido multimedia.
        /// </summary>
        /// <param name="content">Contenido multimedia.</param>
        /// <returns>Contenido multimedia de contrato.</returns>
        public MultimediaContent GetMultimediaContentSummary(Domain.Domain.MultimediaContent content)
        {
            return new MultimediaContent()
            {
                FileName = content.Name,
                Reports = content.Reports.Any() ? content.Reports.Select(x => GetReportSummary(x)).ToList() : null,
                ContentType = content.ContentType
            };
        }
    }
}

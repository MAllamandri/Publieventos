﻿namespace PubliEventos.Services.Services
{
    using LinqKit;
    using NHibernate.Linq;
    using PubliEventos.Contract.Services.Invitation;
    using PubliEventos.DataAccess.Querys;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    /// <summary>
    /// Servicios de invitaciones.
    /// </summary>
    public class InvitationServices : BaseService
    {
        /// <summary>
        /// Crea una invitación.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static CreateInvitationResponse CreateInvitation(CreateInvitationRequest request)
        {
            CreateInvitation(request.UserIds, request.GroupId, request.EventId);

            return new CreateInvitationResponse();
        }

        /// <summary>
        /// Obtiene las invitaciones de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static SearchInvitationsByUserResponse SearchInvitationsByUser(SearchInvitationsByUserRequest request)
        {
            var invitations = CurrentSession.Query<Domain.Domain.Invitation>().Where(x => !x.NullDate.HasValue && !x.Confirmed.HasValue && x.User.Id == request.UserId).Select(x => InternalServices.GetInvitationSummary(x)).ToList();

            return new SearchInvitationsByUserResponse()
            {
                Invitations = invitations
            };
        }

        /// <summary>
        /// Registra la respuesta a la invitación.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static ReplyInvitationResponse ReplyInvitation(ReplyInvitationRequest request)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                var invitation = CurrentSession.Query<Domain.Domain.Invitation>().Where(x => !x.NullDate.HasValue && x.Id == request.InvitationId).FirstOrDefault();

                invitation.Confirmed = request.Reply;

                // Si la invitación fue a un grupo, activo el usuario.
                if (invitation.Group != null && !invitation.Group.NullDate.HasValue)
                {
                    var userGroup = invitation.Group.UsersGroup.Where(x => x.UserId == invitation.User.Id && !x.NullDate.HasValue && !x.Active.HasValue).SingleOrDefault();

                    userGroup.Active = request.Reply;
                    userGroup.NullDate = !request.Reply ? DateTime.Now : (DateTime?)null;

                }

                transaction.Complete();

                return new ReplyInvitationResponse();
            }
        }

        /// <summary>
        /// Obtiene invitaciones de un evento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static SearchInvitationsByEventResponse SearchInvitationsByEvent(SearchInvitationsByEventRequest request)
        {
            var invitations = CurrentSession.Query<Domain.Domain.Invitation>().Where(x => !x.NullDate.HasValue && x.Event.Id == request.EventId).Select(x => InternalServices.GetInvitationSummary(x)).ToList();

            return new SearchInvitationsByEventResponse()
            {
                Invitations = invitations
            };
        }

        /// <summary>
        /// Obtiene los eventos a los que el usuario confirmó su asistencia.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static SearchEventsUserConfirmedResponse SearchEventsUserConfirmed(SearchEventsUserConfirmedRequest request)
        {
            var predicate = PredicateBuilder.True<Domain.Domain.Invitation>();
            predicate = predicate.And(x => !x.NullDate.HasValue && x.Confirmed.Value && x.Event != null);

            if (request.UserId.HasValue)
            {
                predicate = predicate.And(x => x.User.Id == request.UserId.Value);
            }

            if (request.EventTypeId.HasValue)
            {
                predicate = predicate.And(x => x.Event.EventType.Id == request.EventTypeId.Value);
            }

            if (request.EndDate.HasValue && request.StartDate.HasValue)
            {
                predicate = predicate.And(x => x.Event.EventDate.Date >= request.StartDate.Value.Date && x.Event.EventDate.Date <= request.EndDate.Value.Date);
            }
            else if (request.StartDate.HasValue)
            {
                predicate = predicate.And(x => x.Event.EventDate.Date >= request.StartDate.Value.Date);
            }
            else if (request.EndDate.HasValue)
            {
                predicate = predicate.And(x => x.Event.EventDate.Date <= request.EndDate.Value.Date);
            }

            var events = CurrentSession.Query<Domain.Domain.Invitation>()
                            .Where(predicate)
                            .Select(x => InternalServices.GetEventSummary(x.Event))
                            .Take(500)
                            .ToList();

            events = events.Any() ? events.OrderByDescending(x => x.EventDate.Date).ToList() : events;

            return new SearchEventsUserConfirmedResponse
            {
                Events = events
            };
        }

        /// <summary>
        /// Marca la asistencia o cancelación de asistencia de un usuario a un evento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static AttendEventResponse AttendEvent(AttendEventRequest request)
        {
            var ids = new List<int>();
            ids.Add(request.UserId);

            CreateInvitation(ids, null, request.EventId);

            return new AttendEventResponse();
        }

        #region Private Methods

        /// <summary>
        /// Crea una invitación.
        /// </summary>
        /// <param name="userId">Usuario invitado.</param>
        /// <param name="groupId">Identificador del grupo.</param>
        /// <param name="eventId">Identificador del evento.</param>
        private static void CreateInvitation(List<int> userIds, int? groupId, int? eventId)
        {
            foreach (var userId in userIds)
            {
                var invitation = new Domain.Domain.Invitation()
                {
                    Event = eventId.HasValue ? CurrentSession.Get<Domain.Domain.Event>(eventId.Value) : null,
                    Group = groupId.HasValue ? CurrentSession.Get<Domain.Domain.Group>(groupId.Value) : null,
                    EffectDate = DateTime.Now,
                    User = CurrentSession.Get<Domain.Domain.User>(userId)
                };

                new BaseQuery<Domain.Domain.Invitation, int>().Create(invitation);
            }
        }

        #endregion
    }
}

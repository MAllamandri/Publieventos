namespace PubliEventos.Services.Services
{
    using NHibernate.Linq;
    using PubliEventos.Contract.Services.Invitation;
    using PubliEventos.DataAccess.Querys;
    using System;
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
            var invitation = new Domain.Domain.Invitation()
            {
                Event = request.EventId.HasValue ? CurrentSession.Get<Domain.Domain.Event>(request.EventId) : null,
                Group = request.GroupId.HasValue ? CurrentSession.Get<Domain.Domain.Group>(request.GroupId) : null,
                EffectDate = DateTime.Now,
                User = CurrentSession.Get<Domain.Domain.User>(request.UserId)
            };

            new BaseQuery<Domain.Domain.Invitation, int>().Create(invitation);

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
    }
}

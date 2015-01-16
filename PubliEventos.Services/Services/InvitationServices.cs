namespace PubliEventos.Services.Services
{
    using PubliEventos.Contract.Services.Invitation;
    using PubliEventos.DataAccess.Querys;
    using System;

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
    }
}

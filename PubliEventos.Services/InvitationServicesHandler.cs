namespace PubliEventos.Services
{
    using PubliEventos.Contract.Contracts;
    using Contract.Services.Invitation;
    using PubliEventos.Services.Services;

    /// <summary>
    /// Manejador de servicio de invitaciones.
    /// </summary>
    public class InvitationServicesHandler : IInvitationServices
    {
        /// <summary>
        /// Crea una invitación.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public CreateInvitationResponse CreateInvitation(CreateInvitationRequest request)
        {
            return InvitationServices.CreateInvitation(request);
        }

        /// <summary>
        /// Obtiene las invitaciones de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public SearchInvitationsByUserResponse SearchInvitationsByUser(SearchInvitationsByUserRequest request)
        {
            return InvitationServices.SearchInvitationsByUser(request);
        }

        /// <summary>
        /// Registra la respuesta a la invitación.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public ReplyInvitationResponse ReplyInvitation(ReplyInvitationRequest request)
        {
            return InvitationServices.ReplyInvitation(request);
        }

        /// <summary>
        /// Obtiene invitaciones de un evento.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public SearchInvitationsByEventResponse SearchInvitationsByEvent(SearchInvitationsByEventRequest request)
        {
            return InvitationServices.SearchInvitationsByEvent(request);
        }

        /// <summary>
        /// Obtiene los eventos a los que el usuario confirmó su asistencia.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public SearchEventsUserConfirmedResponse SearchEventsUserConfirmed(SearchEventsUserConfirmedRequest request)
        {
            return InvitationServices.SearchEventsUserConfirmed(request);
        }
    }
}

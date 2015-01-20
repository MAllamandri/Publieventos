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
    }
}

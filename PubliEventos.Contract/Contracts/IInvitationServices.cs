namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Services.Invitation;

    /// <summary>
    /// Servicio de invitaciones.
    /// </summary>
    public interface IInvitationServices
    {
        /// <summary>
        /// Crea una invitación.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        CreateInvitationResponse CreateInvitation(CreateInvitationRequest request);

        /// <summary>
        /// Obtiene las invitaciones de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        SearchInvitationsByUserResponse SearchInvitationsByUser(SearchInvitationsByUserRequest request);
    }
}

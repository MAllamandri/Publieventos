namespace PubliEventos.Contract.Services.Invitation
{
    using Contract.Class;
    using System.Collections.Generic;

    /// <summary>
    /// Salidas de la operación SearchInvitationsByUser.
    /// </summary>
    public class SearchInvitationsByUserResponse
    {
        /// <summary>
        /// Invitaciones.
        /// </summary>
        public List<Invitation> Invitations { get; set; }
    }
}

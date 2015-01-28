namespace PubliEventos.Contract.Services.Invitation
{
    using Contract.Class;
    using System.Collections.Generic;

    /// <summary>
    /// Salidas de la operación SearchInvitationsByEvent.
    /// </summary>
    public class SearchInvitationsByEventResponse
    {
        /// <summary>
        /// Invitaciones.
        /// </summary>
        public List<Invitation> Invitations { get; set; }
    }
}

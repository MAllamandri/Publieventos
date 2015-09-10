namespace PubliEventos.Contract.Services.Invitation
{
    /// <summary>
    /// Salidas de la operación HasUserPendingInvitations.
    /// </summary>
    public class HasUserPendingInvitationsResponse
    {
        /// <summary>
        /// Indica si tiene invitaciones pendientes.
        /// </summary>
        public bool HasInvitations { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Invitation
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación HasUserPendingInvitations.
    /// </summary>
    public class HasUserPendingInvitationsRequest
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Invitation
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación SearchInvitationsByUser.
    /// </summary>
    public class SearchInvitationsByUserRequest
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Invitation
{
    /// <summary>
    /// Representa los parámetros de la operación ReplyInvitation.
    /// </summary>
    public class ReplyInvitationRequest
    {
        /// <summary>
        /// Identificador de la invitación.
        /// </summary>
        public int InvitationId { get; set; }

        /// <summary>
        /// Respuesta a la invitación.
        /// </summary>
        public bool Reply { get; set; }
    }
}

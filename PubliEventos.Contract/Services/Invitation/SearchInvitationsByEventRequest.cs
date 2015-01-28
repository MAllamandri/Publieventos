namespace PubliEventos.Contract.Services.Invitation
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación SearchInvitationsByEvent.
    /// </summary>
    public class SearchInvitationsByEventRequest
    {
        /// <summary>
        /// Identificador del evento.
        /// </summary>
        public int EventId { get; set; }
    }
}

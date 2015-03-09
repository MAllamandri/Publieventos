namespace PubliEventos.Contract.Services.Comment
{
    /// <summary>
    /// Request del servicio GetCommentsByEvent.
    /// </summary>
    public class GetCommentsByEventRequest
    {
        /// <summary>
        /// Identificador del evento.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Identificador del usuario actual.
        /// </summary>
        public int CurrentUserId { get; set; }
    }
}

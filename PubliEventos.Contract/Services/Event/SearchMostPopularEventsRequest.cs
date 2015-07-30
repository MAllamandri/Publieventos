namespace PubliEventos.Contract.Services.Event
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación SearchMostPopularEvents.
    /// </summary>
    public class SearchMostPopularEventsRequest
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }
    }
}

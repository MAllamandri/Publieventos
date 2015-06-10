namespace PubliEventos.Contract.Services.Invitation
{
    using System.Collections.Generic;
    using PubliEventos.Contract.Class;

    /// <summary>
    /// Salidas de la operación SearchEventsUserConfirmed.
    /// </summary>
    public class SearchEventsUserConfirmedResponse
    {
        /// <summary>
        /// Eventos a los que asisitio.
        /// </summary>
        public List<Event> Events { get; set; }
    }
}

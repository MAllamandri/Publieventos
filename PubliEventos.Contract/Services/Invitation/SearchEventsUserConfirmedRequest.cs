namespace PubliEventos.Contract.Services.Invitation
{
    using System;

    /// <summary>
    /// Representa los parámetros de entrada de la operación.
    /// </summary>
    public class SearchEventsUserConfirmedRequest
    {
        /// <summary>
        /// Identificador del tipo de evento.
        /// </summary>
        public int? EventTypeId { get; set; }

        /// <summary>
        /// Fecha desde para búsqueda.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Fecha hasta para búsqueda.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Usuario que confirmó.
        /// </summary>
        public int? UserId { get; set; }
    }
}

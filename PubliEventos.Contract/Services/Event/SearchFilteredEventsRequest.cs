﻿namespace PubliEventos.Contract.Services.Event
{
    using System;

    /// <summary>
    /// Parametros para la busqueda de eventos por diferentes filtros.
    /// </summary>
    public class SearchFilteredEventsRequest
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
        /// Identificador del usuario para filtrar eventos.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Indica que busca los eventos publicos.
        /// </summary>
        public bool SearchPublics { get; set; }

        /// <summary>
        /// Indica que busca los eventos privados.
        /// </summary>
        public bool SearchPrivate { get; set; }

        /// <summary>
        /// Término de búsqueda.
        /// </summary>
        public string SearchTerm { get; set; }
    }
}

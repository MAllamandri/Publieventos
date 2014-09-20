namespace PubliEventos.Contract.Services
{
    using System;

    /// <summary>
    /// Representa un los paramtros de entrada de la operación EventCreate.
    /// </summary>
    public class EventCreateRequest
    {
        /// <summary>
        /// Título del evento.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Detalle del evento.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Descripcion del evento.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Indica si es privado o público.
        /// </summary>
        public bool Private { get; set; }

        /// <summary>
        /// Fecha del evento.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Hora del evento.
        /// </summary>
        public string EventTime { get; set; }

        /// <summary>
        /// Foto de portada.
        /// </summary>
        public string CoverPhoto { get; set; }

        /// <summary>
        /// Tipo de evento.
        /// </summary>
        public virtual int EventTypeId { get; set; }

        /// <summary>
        /// Localidad del evento.
        /// </summary>
        public int LocalityId { get; set; }

        /// <summary>
        /// Indica si esta activo.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Usuario creador del evento.
        /// </summary>
        public int UserId { get; set; }
    }
}

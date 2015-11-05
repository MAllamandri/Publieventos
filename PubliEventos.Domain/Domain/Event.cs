namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Representa un evento.
    /// </summary>
    public class Event : BaseIdentifier<int>
    {
        /// <summary>
        /// Título del evento.
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Detalle del evento.
        /// </summary>
        public virtual string Detail { get; set; }

        /// <summary>
        /// Descripcion del evento.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public virtual DateTime CreationDate { get; set; }

        /// <summary>
        /// Indica si es privado o público.
        /// </summary>
        public virtual bool Private { get; set; }

        /// <summary>
        /// Fecha del evento.
        /// </summary>
        public virtual DateTime EventDate { get; set; }

        /// <summary>
        /// Hora de comienzo del evento.
        /// </summary>
        public virtual TimeSpan EventStartTime { get; set; }

        /// <summary>
        /// Hora de fin del evento.
        /// </summary>
        public virtual TimeSpan EventEndTime { get; set; }

        /// <summary>
        /// Foto de portada.
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Tipo de evento.
        /// </summary>
        public virtual EventType EventType { get; set; }

        /// <summary>
        /// Comentarios del evento.
        /// </summary>
        public virtual IList<Comment> Comments { get; set; }

        /// <summary>
        /// Contenidos multimedia relacionados al evento (imagenes o videos).
        /// </summary>
        public virtual IList<MultimediaContent> MultimediaContents { get; set; }

        /// <summary>
        /// Reportes que recibio el evento.
        /// </summary>
        public virtual IList<Report> Reports { get; set; }

        /// <summary>
        /// Indica si esta activo.
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// Usuario creador del evento.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Indica si está dado de baja.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }

        /// <summary>
        /// Latitud.
        /// </summary>
        public virtual string Latitude { get; set; }

        /// <summary>
        /// Longitud.
        /// </summary>
        public virtual string Longitude { get; set; }

        /// <summary>
        /// Cantidad de veces que fue visto el evento.
        /// </summary>
        public virtual int Views { get; set; }
    }
}

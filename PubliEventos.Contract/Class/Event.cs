using System.Timers;

namespace PubliEventos.Contract.Class
{
    using System;

    public class Event
    {
        /// <summary>
        /// Identificador del evento.
        /// </summary>
        public int? Id { get; set; }

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
        /// Hora de comienzo del evento.
        /// </summary>
        public TimeSpan EventStartTime { get; set; }

        /// <summary>
        /// Hora de fin del evento.
        /// </summary>
        public TimeSpan EventEndTime { get; set; }

        /// <summary>
        /// Foto de portada.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Tipo de evento.
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// Comentarios del evento.
        /// </summary>
        //public virtual IList<Comment> Comments { get; set; }

        /// <summary>
        /// Contenidos multimedia relacionados al evento (imagenes o videos).
        /// </summary>
        //public virtual IList<MultimediaContent> MultimediaContents { get; set; }

        /// <summary>
        /// Localidad del evento.
        /// </summary>
        public Locality Locality { get; set; }

        /// <summary>
        /// Indica si esta activo.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Usuario creador del evento.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Latitud.
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Longitud.
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Indica si está dado de baja.
        /// </summary>
        public DateTime NullDate { get; set; }
    }
}

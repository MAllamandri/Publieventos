namespace PubliEventos.Contract.Class
{
    using System;

    public class Event
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
        //public virtual EventType EventType { get; set; }

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
        public virtual Locality Locality { get; set; }

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
        public virtual DateTime NullDate { get; set; }
    }
}

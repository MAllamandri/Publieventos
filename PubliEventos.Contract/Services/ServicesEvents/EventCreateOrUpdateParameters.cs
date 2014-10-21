namespace PubliEventos.Contract.Services.ServicesEvents
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    /// <summary>
    /// Representa un los paramtros de entrada de la operación EventCreate.
    /// </summary>
    public class EventCreateOrUpdateRequest
    {
        /// <summary>
        /// Identificador del evento.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Título del evento.
        /// </summary>
        [Required]
        [Display(Name = "Título")]
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
        /// Indica si es privado o público.
        /// </summary>
        public bool Private { get; set; }

        /// <summary>
        /// Fecha del evento.
        /// </summary>
        [Required]
        [Display(Name = "Fecha de Realización")]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es correcto")]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Hora comienzo del evento.
        /// </summary>
        [Required]
        [Display(Name = "Hora de Inicio")]
        [DataType(DataType.Time, ErrorMessage = "El formato de la hora no es correcto")]
        public TimeSpan EventStartTime { get; set; }

        /// <summary>
        /// Hora de fin del evento.
        /// </summary>
        [Required]
        [Display(Name = "Hora de Fin")]
        [DataType(DataType.Time, ErrorMessage = "El formato de la hora no es correcto")]
        public TimeSpan EventEndTime { get; set; }

        /// <summary>
        /// Foto de portada.
        /// </summary>
        public HttpPostedFileBase CoverPhoto { get; set; }

        /// <summary>
        /// Nombre de la foto de portada.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Tipo de evento.
        /// </summary>
        [Required]
        [Display(Name = "Tipo de Evento")]
        public virtual int EventTypeId { get; set; }

        /// <summary>
        /// Localidad del evento.
        /// </summary>
        [Display(Name = "Localidad")]
        public int LocalityId { get; set; }

        /// <summary>
        /// Localidad del evento.
        /// </summary>
        [Display(Name = "Provincia")]
        public int ProvinceId { get; set; }

        /// <summary>
        /// Indica si esta activo.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Usuario creador.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Latitud de ubicación.
        /// </summary>
        [Required]
        public string Latitude { get; set; }

        /// <summary>
        /// Longitud de ubicación.
        /// </summary>
        [Required]
        public string Longitude { get; set; }
    }
}

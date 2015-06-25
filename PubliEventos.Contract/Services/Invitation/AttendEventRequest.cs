namespace PubliEventos.Contract.Services.Invitation
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Representa los parámetros de entrada la operación AttendEvent.
    /// </summary>
    public class AttendEventRequest
    {
        /// <summary>
        /// Usuario que marco su asistencia o cancelación de asistencia.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Identificador del evento.
        /// </summary>
        [Required]
        public int EventId { get; set; }
    }
}

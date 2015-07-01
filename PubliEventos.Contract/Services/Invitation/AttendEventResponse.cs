namespace PubliEventos.Contract.Services.Invitation
{
    using PubliEventos.Contract.Class;

    /// <summary>
    /// Salidas de la operación AttendEvent.
    /// </summary>
    public class AttendEventResponse
    {
        /// <summary>
        /// Indica si el usuario asistira al evento.
        /// </summary>
        public bool Attend { get; set; }

        /// <summary>
        /// Usuario que marco asistencia.
        /// </summary>
        public User User { get; set; }
    }
}

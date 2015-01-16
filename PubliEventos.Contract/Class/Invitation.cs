namespace PubliEventos.Contract.Class
{
    /// <summary>
    /// Representa una invitación.
    /// </summary>
    public class Invitation
    {
        /// <summary>
        /// Grupo.
        /// </summary>
        public Group Group { get; set; }

        /// <summary>
        /// Evento.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// Usuario invitado.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Indica si esta confirmado o no.
        /// </summary>
        public bool? Confirmed { get; set; }
    }
}

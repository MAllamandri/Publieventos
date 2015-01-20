namespace PubliEventos.Contract.Class
{
    /// <summary>
    /// Representa un usuario de un grupo.
    /// </summary>
    public class UserGroup
    {
        /// <summary>
        /// Identificador del Grupo.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Indica si el usuario esta activo.
        /// </summary>
        public bool? Active { get; set; }
    }
}

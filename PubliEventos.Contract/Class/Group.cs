namespace PubliEventos.Contract.Class
{
    using System.Collections.Generic;

    /// <summary>
    /// Representa un grupo de usuarios.
    /// </summary>
    public class Group : BaseClass
    {
        /// <summary>
        /// Nombre del grupo.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mensaje al grupo.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Usuarios administrador.
        /// </summary>
        public User Administrator { get; set; }

        /// <summary>
        /// Usuarios del grupo.
        /// </summary>
        public List<User> Users { get; set; }
    }
}

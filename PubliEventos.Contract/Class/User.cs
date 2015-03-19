namespace PubliEventos.Contract.Class
{
    using System;
    using PubliEventos.Domain.Domain;

    /// <summary>
    /// Representa un usuario.
    /// </summary>
    public class User : BaseClass
    {
        /// <summary>
        /// Usuario de la aplicación.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Contraseña.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Fecha de nacimiento.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Fecha de alta.
        /// </summary>
        public DateTime? EffectDate { get; set; }

        /// <summary>
        /// Email del usuario.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Localidad donde vive el usuario.
        /// </summary>
        public Locality Locality { get; set; }

        /// <summary>
        /// Indica si el usuario esta activo.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Indica si el usuario esta dado de baja.
        /// </summary>
        public DateTime? NullDate { get; set; }

        /// <summary>
        /// Imagen de perfil.
        /// </summary>
        public string ImageProfile { get; set; }

        /// <summary>
        /// Indica si el usuario es administrador.
        /// </summary>
        public bool IsAdministrator { get; set; }
    }
}

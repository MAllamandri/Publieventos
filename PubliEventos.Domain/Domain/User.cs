namespace PubliEventos.Domain.Domain
{
    using System;
    using PubliEventos.DataAccess.Infrastructure;

    /// <summary>
    /// Representa un usuario de la comunidad.
    /// </summary>
    public class User : BaseIdentifier<int>
    {
        /// <summary>
        /// Usuario de la aplicación.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Contraseña.
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Fecha de nacimiento.
        /// </summary>
        public virtual DateTime BirthDate { get; set; }

        /// <summary>
        /// Email del usuario.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Localidad donde vive el usuario.
        /// </summary>
        public virtual Locality Locality { get; set; }

        /// <summary>
        /// Indica si el usuario esta activo.
        /// </summary>
        public virtual bool Active { get; set; }

        /// <summary>
        /// Indica si el usuario esta dado de baja.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }

        /// <summary>
        /// Imagen de perfil.
        /// </summary>
        public virtual string ImageProfile { get; set; }
    }
}

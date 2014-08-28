namespace PubliEventos.Web.App_Start
{
    using System.Security.Principal;
    using System.Web.Security;

    /// <summary>
    /// Interface que contiene las propiedad que tendra el usuario.
    /// </summary>
    interface ICustomPrincipal : IPrincipal
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Imagen de perfil.
        /// </summary>
        string ImageProfile { get; set; }
    }

    /// <summary>
    /// Representa un usuario logueado.
    /// </summary>
    public class CustomPrincipal : ICustomPrincipal
    {
        #region Properties

        /// <summary>
        /// Identity.
        /// </summary>
        public IIdentity Identity { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="username">Usuario a identificar.</param>
        public CustomPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        #endregion

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Imagen de perfil.
        /// </summary>
        public string ImageProfile { get; set; }

        /// <summary>
        /// Indica si un usuario se encuentra en dicho rol.
        /// </summary>
        /// <param name="role">Rol a verificar.</param>
        /// <returns>True si se encuentra en el rol.</returns>
        public bool IsInRole(string role)
        {
            return Identity != null && Identity.IsAuthenticated &&
               !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role);
        }
    }

    /// <summary>
    /// Representa un usuario logueado.
    /// </summary>
    public class CustomPrincipalSerializeModel
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Imagen de perfil.
        /// </summary>
        public string ImageProfile { get; set; }
    }
}
namespace PubliEventos.Web.Models.AccountModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo que representa un usuario.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        /// <summary>
        /// Contraseña.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        /// <summary>
        /// Modelo de registración.
        /// </summary>
        public SignUpModel SignUpModel { get; set; }

        /// <summary>
        /// Indica si se recuerda la cuenta o no.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Indica si es un login.
        /// </summary>
        public bool IsLogin { get; set; }
    }
}
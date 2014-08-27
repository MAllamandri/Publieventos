namespace PubliEventos.Web.Models.AccountModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de login.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Contraseña.
        /// </summary>
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Indica si se recuerda la cuenta o no.
        /// </summary>
        public string RememberMe { get; set; }
    }
}
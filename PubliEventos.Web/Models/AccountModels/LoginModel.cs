namespace PubliEventos.Web.Models.AccountModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Modelo de login.
    /// </summary>
    public class LoginModel : UserModel
    {
        /// <summary>
        /// Indica si se recuerda la cuenta o no.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
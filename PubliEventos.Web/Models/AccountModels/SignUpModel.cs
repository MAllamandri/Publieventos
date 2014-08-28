namespace PubliEventos.Web.Models.AccountModels
{
    using System.ComponentModel.DataAnnotations;

    public class SignUpModel : UserModel
    {
        /// <summary>
        /// Repeticion de la contraseña.
        /// </summary>
        [DataType(DataType.Password)]
        [CompareAttribute("Password", ErrorMessage = "Las contraseñas no son iguales.")]
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Email del usuario.
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
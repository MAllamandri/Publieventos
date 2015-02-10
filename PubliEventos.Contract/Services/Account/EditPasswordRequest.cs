namespace PubliEventos.Contract.Services.Account
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Representa los parámetros de la operación EditPassword.
    /// </summary>
    public class EditPasswordRequest
    {
        /// <summary>
        /// Identificador del usuario a cambiar el password.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Password actual.
        /// </summary>
        [Required]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Password actual a comparar.
        /// </summary>
        [Required]
        [Display(Name = "Contraseña Actual")]
        [CompareAttribute("CurrentPassword", ErrorMessage = "Las contraseña actual no coincide")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Nueva contraseña.
        /// </summary>
        [Required]
        [Display(Name = "Nueva Contraseña")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener mas de 8 caracteres.")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Repetición de nueva contraseña.
        /// </summary>
        [Required]
        [Display(Name = "Repetir Contraseña")]
        [CompareAttribute("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string RepeatPassword { get; set; }
    }
}

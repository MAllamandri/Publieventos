namespace PubliEventos.Contract.Services.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Representa los parámetros de la operación EditPassword.
    /// </summary>
    public class EditPasswordRequest : IValidatableObject
    {
        /// <summary>
        /// Identificador del usuario a cambiar el password.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public int UserId { get; set; }

        /// <summary>
        /// Password actual.
        /// </summary>
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Password actual a comparar.
        /// </summary>
        [Display(Name = "Contraseña Actual")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Nueva contraseña.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Nueva Contraseña")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener mas de 8 caracteres.")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Repetición de nueva contraseña.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Repetir Contraseña")]
        [CompareAttribute("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Indica si viene el password actual.
        /// </summary>
        public bool ValidateCurrentPassword { get; set; }

        /// <summary>
        /// Validaciones.
        /// </summary>
        /// <param name="validationContext">Contexto.</param>
        /// <returns>Resultados de las validaciones.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ValidateCurrentPassword)
            {
                if (string.IsNullOrEmpty(this.OldPassword))
                {
                    yield return new ValidationResult("El campo contraseña actual es obligatorio.", new[] { "OldPassword" });
                }

                if (!string.IsNullOrEmpty(this.OldPassword) && this.OldPassword != this.CurrentPassword)
                {
                    yield return new ValidationResult("La contraseña actual no coincide.", new[] { "OldPassword" });
                }
            }

            if (!this.NewPassword.Any(x => char.IsUpper(x)))
            {
                yield return new ValidationResult("La contraseña debe contener al menos una mayúscula.", new[] { "NewPassword" });
            }

            if (!this.NewPassword.Any(char.IsDigit))
            {
                yield return new ValidationResult("La contraseña debe contener al menos un número.", new[] { "NewPassword" });
            }
        }
    }
}

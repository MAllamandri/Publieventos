namespace PubliEventos.Web.Models.AccountModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Mail;

    /// <summary>
    /// Modelo que representa un usuario.
    /// </summary>
    public class UserModel : IValidatableObject
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        /// <summary>
        /// Contraseña.
        /// </summary>
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        /// <summary>
        /// Repeticion de la contraseña.
        /// </summary>
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Email del usuario.
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Localidad del usuario.
        /// </summary>
        public int? Locality { get; set; }

        /// <summary>
        /// Indica si se recuerda la cuenta o no.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Indica si es un login o una registración.
        /// </summary>
        public bool IsLogin { get; set; }

        #region self-validators

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.IsLogin)
            {
                if (string.IsNullOrEmpty(this.RepeatPassword))
                {
                    yield return new ValidationResult("Las contraseña es requerida", new[] { "Password" });
                }
                else if (!this.RepeatPassword.Equals(this.Password))
                {
                    yield return new ValidationResult("Las contraseñas no son iguales", new[] { "RepeatPassword" });
                }

                if (!this.Locality.HasValue)
                {
                    yield return new ValidationResult("La localidad es obligatoria", new[] { "Locality" });
                }

                if (string.IsNullOrEmpty(this.Email))
                {
                    yield return new ValidationResult("El Email es requerido", new[] { "Email" });
                }
                else if (!this.IsValid(this.Email))
                {
                    yield return new ValidationResult("Email inválido", new[] { "Email" });
                }
            }
        }

        /// <summary>
        /// Valida un email.
        /// </summary>
        /// <param name="emailaddress">email.</param>
        /// <returns>True si es valido.</returns>
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        #endregion
    }
}
﻿namespace PubliEventos.Web.Models.AccountModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Mail;
    using System.Linq;

    public class SignUpModel : IValidatableObject
    {
        /// <summary>
        /// Nombre.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Nombre")]
        [StringLength(20, ErrorMessage = "No puede superar los 20 caracteres.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Apellido")]
        [StringLength(20, ErrorMessage = "No puede superar los 20 caracteres.")]
        public string LastName { get; set; }

        /// <summary>
        /// UserName a registrar
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Usuario")]
        public string UserNameToRegister { get; set; }

        /// <summary>
        /// Password a registrar.
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Contraseña")]
        [MinLength(8, ErrorMessage = "La contraseña debe contener mas de 8 caracteres.")]
        public string PasswordToRegister { get; set; }

        /// <summary>
        /// Repeticion de la contraseña.
        /// </summary>
        [DataType(DataType.Password)]
        [CompareAttribute("PasswordToRegister", ErrorMessage = "Las contraseñas no son iguales.")]
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Repetir Contraseña")]
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Email del usuario.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [EmailAddress(ErrorMessage = "El email no es valido.")]
        public string Email { get; set; }

        /// <summary>
        /// Localidad del usuario.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Localidad")]
        public int? Locality { get; set; }

        /// <summary>
        /// Fecha de nacimiento.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Validaciones.
        /// </summary>
        /// <param name="validationContext">Contexto.</param>
        /// <returns>Resultados de las validaciones.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.PasswordToRegister.Any(x => char.IsUpper(x)))
            {
                yield return new ValidationResult("La contraseña debe contener al menos una mayúscula.", new[] { "PasswordToRegister" });
            }

            if (!this.PasswordToRegister.Any(char.IsDigit))
            {
                yield return new ValidationResult("La contraseña debe contener al menos un número.", new[] { "PasswordToRegister" });
            }
        }
    }
}
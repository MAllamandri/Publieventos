﻿namespace PubliEventos.Contract.Services.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    /// <summary>
    /// Representa los parámetros de entrada de la operación EditProfile.
    /// </summary>
    public class EditProfileRequest
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        [StringLength(30, ErrorMessage = "No puede superar los 30 caracteres.")]
        [Display(Name = "Nombre de Usuario")]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Fecha de nacimiento.
        /// </summary>
        [Display(Name = "Fecha de Nacimiento")]
        [Required]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Nombre.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        [EmailAddress(ErrorMessage = "Formato de email incorrecto")]
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Identificador de la localidad.
        /// </summary>
        [Required]
        [Display(Name = "Localidad")]
        public int LocalityId { get; set; }

        /// <summary>
        /// Identificador de la provincia.
        /// </summary>
        [Required]
        [Display(Name = "Provincia")]
        public int ProvinceId { get; set; }

        /// <summary>
        /// Imagen de perfil.
        /// </summary>
        public string ImageProfile { get; set; }

        /// <summary>
        /// Foto de portada.
        /// </summary>
        public HttpPostedFileBase ImageFile { get; set; }
    }
}

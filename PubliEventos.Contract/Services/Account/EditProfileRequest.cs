namespace PubliEventos.Contract.Services.Account
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
        [Required(ErrorMessage = "El valor es requerido")]
        public int UserId { get; set; }

        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        [StringLength(30, ErrorMessage = "No puede superar los 30 caracteres.")]
        [Display(Name = "Nombre de Usuario")]
        [Required(ErrorMessage = "El valor es requerido")]
        public string UserName { get; set; }

        /// <summary>
        /// Fecha de nacimiento.
        /// </summary>
        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "El valor es requerido")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Nombre.
        /// </summary>
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor es requerido")]
        [MaxLength(25, ErrorMessage = "El nombre no puede contener mas de 25 caracteres")]
        public string FirstName { get; set; }

        /// <summary>
        /// Apellido.
        /// </summary>
        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El valor es requerido")]
        [MaxLength(25, ErrorMessage = "El apellido no puede contener mas de 25 caracteres")]
        public string LastName { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        [EmailAddress(ErrorMessage = "Formato de email incorrecto")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El valor es requerido")]
        public string Email { get; set; }

        /// <summary>
        /// Identificador de la localidad.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Localidad")]
        public int LocalityId { get; set; }

        /// <summary>
        /// Identificador de la provincia.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Provincia")]
        public int ProvinceId { get; set; }

        /// <summary>
        /// Imagen de perfil.
        /// </summary>
        public string PathProfile { get; set; }

        /// <summary>
        /// Contraseña.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// FileName de la imagen de perfil.
        /// </summary>
        public string ImageProfile { get; set; }

        /// <summary>
        /// Nombre anterior.
        /// </summary>
        public string FirstNameOld { get; set; }
    }
}

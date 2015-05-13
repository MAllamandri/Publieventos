namespace PubliEventos.Contract.Services.Group
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Base request para la creación y edición de grupos.
    /// </summary>
    public class GroupBaseRequest
    {
        /// <summary>
        /// Nombre del grupo.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Nombre del Grupo")]
        public string GroupName { get; set; }

        /// <summary>
        /// Mensaje al grupo.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [Display(Name = "Mensaje al Grupo")]
        public string Message { get; set; }

        /// <summary>
        /// Identificador del usuario administrador.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public int AdministratorId { get; set; }

        /// <summary>
        /// Usuarios a invitar al grupo.
        /// </summary>
        [Required(ErrorMessage = "Invite algún usuario al grupo")]
        [Display(Name = "Usuarios del Grupo")]
        public string UserIds { get; set; }
    }
}

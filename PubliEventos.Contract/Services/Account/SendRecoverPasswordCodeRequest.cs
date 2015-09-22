namespace PubliEventos.Contract.Services.Account
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Representa los parámetros de entrada de la operación SendRecoverPasswordCode.
    /// </summary>
    public class SendRecoverPasswordCodeRequest
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        public string UserName { get; set; }

        /// <summary>
        /// Email del usuario.
        /// </summary>
        [Required(ErrorMessage = "El valor es requerido")]
        [EmailAddress]
        public string Email { get; set; }
    }
}

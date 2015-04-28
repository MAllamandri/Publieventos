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
        [Required]
        public string UserName { get; set; }
    }
}

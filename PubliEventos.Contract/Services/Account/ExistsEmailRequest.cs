namespace PubliEventos.Contract.Services.Account
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación ExistsEmail.
    /// </summary>
    public class ExistsEmailRequest
    {
        /// <summary>
        /// Email a validar.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Id del usuario a excluir en la validación.
        /// </summary>
        public int? UserId { get; set; }
    }
}

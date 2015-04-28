namespace PubliEventos.Contract.Services.Account
{
    /// <summary>
    /// Salidas de la operación SendRecoverPasswordCode.
    /// </summary>
    public class SendRecoverPasswordCodeResponse
    {
        /// <summary>
        /// Indica si el envio fue satisfactorio.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int? UserId { get; set; }
    }
}

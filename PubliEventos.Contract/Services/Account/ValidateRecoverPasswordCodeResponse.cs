namespace PubliEventos.Contract.Services.Account
{
    /// <summary>
    /// Salidas de la operación ValidateRecoverPasswordCode.
    /// </summary>
    public class ValidateRecoverPasswordCodeResponse
    {
        /// <summary>
        /// Indica si el código es valido.
        /// </summary>
        public bool IsValid { get; set; }
    }
}

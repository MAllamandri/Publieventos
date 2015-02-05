namespace PubliEventos.Contract.Services.Account
{
    /// <summary>
    /// Salidas de la operación ExistsEmail.
    /// </summary>
    public class ExistsEmailResponse
    {
        /// <summary>
        /// Indica si ya existe un usuario con ese email.
        /// </summary>
        public bool Exists { get; set; }
    }
}

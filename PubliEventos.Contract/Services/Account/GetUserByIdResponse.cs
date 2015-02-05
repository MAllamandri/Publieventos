namespace PubliEventos.Contract.Services.Account
{
    using PubliEventos.Contract.Class;

    /// <summary>
    /// Salidas de la operación GetUserById.
    /// </summary>
    public class GetUserByIdResponse
    {
        /// <summary>
        /// Usuario.
        /// </summary>
        public User User { get; set; }
    }
}

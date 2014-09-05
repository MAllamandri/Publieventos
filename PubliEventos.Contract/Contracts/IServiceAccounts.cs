namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.ContractClass;

    /// <summary>
    /// Interface del servicio de cuentas.
    /// </summary>
    public interface IServiceAccounts
    {
        /// <summary>
        /// Obtiene usuario por userName.
        /// </summary>
        /// <param name="userName">userName del usuario.</param>
        /// <returns>User.</returns>
        User GetUserByUserName(string userName);

        /// <summary>
        /// Da de alta un usuario.
        /// </summary>
        /// <param name="user">usuario.</param>
        int RegisterUser(User user);
    }
}

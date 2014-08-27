namespace PubliEventos.Services
{
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.ContractClass;
    using PubliEventos.Services.Services;

    /// <summary>
    /// Manejador de cuentas.
    /// </summary>
    public class ServicesAccountsHandler : IServiceAccounts
    {
        /// <summary>
        /// Obtiene usuario por userName.
        /// </summary>
        /// <param name="userName">userName del usuario.</param>
        /// <returns></returns>
        public User GetUserByUserName(string userName)
        {
            return ServiceAccounts.GetUserByUserName(userName);
        }
    }
}

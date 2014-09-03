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

        /// <summary>
        /// Da de alta un usuario.
        /// </summary>
        /// <param name="user">Usuario.</param>
        public void CreateUser(User user)
        {
            ServiceAccounts.CreateUser(user);
        }
    }
}

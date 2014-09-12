namespace PubliEventos.Services
{
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Class;
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
        public int RegisterUser(User user)
        {
            return ServiceAccounts.RegisterUser(user);
        }

        /// <summary>
        /// Guarda un token de activación de cuenta.
        /// </summary>
        /// <param name="token">Token.</param>
        public void SaveAccountActivationToken(string token, int idUser)
        {
            ServiceAccounts.SaveAccountActivationToken(token, idUser);
        }

        /// <summary>
        /// Activa una cuenta mediante el token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>True si se activo la cuenta, false caso contrario.</returns>
        public bool ActivateAccount(string token)
        {
            return ServiceAccounts.ActivateAccount(token);
        }

        /// <summary>
        /// Verifica si ya existe un usuario con ese email.
        /// </summary>
        /// <param name="email">Email del usuario.</param>
        /// <returns>True si existe uno, false caso contrario.</returns>
        public bool UserExistsWithEmail(string email)
        {
            return ServiceAccounts.UserExistsWithEmail(email);
        }
    }
}

namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Class;

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
        /// Verifica si ya existe un usuario con ese email.
        /// </summary>
        /// <param name="email">Email del usuario.</param>
        /// <returns>True si existe uno, false caso contrario.</returns>
        bool UserExistsWithEmail(string email);

        /// <summary>
        /// Da de alta un usuario.
        /// </summary>
        /// <param name="user">usuario.</param>
        int RegisterUser(User user);

        /// <summary>
        /// Guarda un token de activación de cuenta.
        /// </summary>
        /// <param name="token">Token.</param>
        void SaveAccountActivationToken(string token, int idUser);

        /// <summary>
        /// Activa una cuenta mediante el token.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <returns>True si se activo la cuenta, false caso contrario.</returns>
        bool ActivateAccount(string token);
    }
}

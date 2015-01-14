namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Services.Account;

    /// <summary>
    /// Interface del servicio de cuentas.
    /// </summary>
    public interface IAccountServices
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

        /// <summary>
        /// Indica si el usuario tiene un token de activación de cuenta activo.
        /// </summary>
        /// <param name="idUser">Identificador del usuario.</param>
        /// <returns>True si posee un token activo, false caso contrario.</returns>
        bool HasActiveActivationToken(int idUser);

        /// <summary>
        /// Doy de baja los token expirados del usuario.
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        void DeleteActivationToken(string userName);

        /// <summary>
        /// Busca usuarios por autocompletado de nombre de usuario.
        /// </summary>
        /// <param name="request">Los parámetros de la búsqueda.</param>
        /// <returns>El resultado de la operación.</returns>
        SearchUsersByPartialUserNameResponse SearchUsersByPartialUserName(SearchUsersByPartialUserNameRequest request);
    }
}

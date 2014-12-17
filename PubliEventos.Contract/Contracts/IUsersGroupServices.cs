namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Services.UsersGroup;

    /// <summary>
    /// Servicio de grupos de usuarios.
    /// </summary>
    public interface IUsersGroupServices
    {
        /// <summary>
        /// Obtiene los grupo de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        GetUsersGroupByUserResponse GetUsersGroupByUser(GetUsersGroupByUserRequest request);
    }
}

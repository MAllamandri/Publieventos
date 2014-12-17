namespace PubliEventos.Services.Services
{
    using PubliEventos.Contract.Services.UsersGroup;
    using System;

    /// <summary>
    /// Servicios de grupos de usuarios.
    /// </summary>
    public class UsersGroupServices : BaseService
    {
        /// <summary>
        /// Obtiene los grupo de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public static GetUsersGroupByUserResponse GetUsersGroupByUser(GetUsersGroupByUserRequest request)
        {
            throw new Exception("No implementado");
        }
    }
}

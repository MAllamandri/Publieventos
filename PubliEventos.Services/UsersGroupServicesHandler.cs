namespace PubliEventos.Services
{
    using Contract.Services.Group;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Services.Services;

    /// <summary>
    /// Manejador de servicio de grupos de usuarios.
    /// </summary>
    public class UsersGroupServicesHandler : IGroupServices
    {
        /// <summary>
        /// Obtiene los grupo de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public GetGroupsByUserResponse GetGroupsByUser(GetGroupsByUserRequest request)
        {
            return GroupServices.GetGroupsByUser(request);
        }

        /// <summary>
        /// Elimina un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public DeleteGroupResponse DeleteGroup(DeleteGroupRequest request)
        {
            return GroupServices.DeleteGroup(request);
        }

        /// <summary>
        /// Da de baja a un usuario de un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        public LeaveGroupResponse LeaveGroup(LeaveGroupRequest request)
        {
            return GroupServices.LeaveGroup(request);
        }
    }
}

namespace PubliEventos.Contract.Contracts
{
    using PubliEventos.Contract.Services.Group;

    /// <summary>
    /// Servicio de grupos de usuarios.
    /// </summary>
    public interface IGroupServices
    {
        /// <summary>
        /// Obtiene los grupo de un usuario.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        GetGroupsByUserResponse GetGroupsByUser(GetGroupsByUserRequest request);

        /// <summary>
        /// Elimina un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        DeleteGroupResponse DeleteGroup(DeleteGroupRequest request);

        /// <summary>
        /// Da de baja a un usuario de un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        LeaveGroupResponse LeaveGroup(LeaveGroupRequest request);
    }
}

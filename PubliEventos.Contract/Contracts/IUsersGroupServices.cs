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

        /// <summary>
        /// Crea un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        CreateGroupResponse CreateGroup(CreateGroupRequest request);

        /// <summary>
        /// Obtiene un grupo por Id.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        GetGroupByIdResponse GetGroupById(GetGroupByIdRequest request);

        /// <summary>
        /// Edita un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        EditGroupResponse EditGroup(EditGroupRequest request);

        /// <summary>
        /// Obtiene grupos por su nombre.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        SearchGroupsByPartialNameResponse SearchGroupsByPartialName(SearchGroupsByPartialNameRequest request);

        /// <summary>
        /// Crea un mensaje de chat de grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        CreateChatMessageResponse CreateChatMessage(CreateChatMessageRequest request);

        /// <summary>
        /// Obtiene los mensajes de chat de un grupo.
        /// </summary>
        /// <param name="request">Los parámetros de entrada.</param>
        /// <returns>El resultado de la operación.</returns>
        SearchChatMessagesByGroupResponse SearchChatMessagesByGroup(SearchChatMessagesByGroupRequest request);
    }
}

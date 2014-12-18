namespace PubliEventos.Contract.Services.Group
{
    /// <summary>
    /// Representa un los paramtros de entrada de la operación GetUsersGroupByUser.
    /// </summary>
    public class GetGroupsByUserRequest
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }
    }
}

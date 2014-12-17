namespace PubliEventos.Contract.Services.UsersGroup
{
    /// <summary>
    /// Representa un los paramtros de entrada de la operación GetUsersGroupByUser.
    /// </summary>
    public class GetUsersGroupByUserRequest
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Group
{
    /// <summary>
    /// Representa un los paramtros de entrada de la operación LeaveGroup.
    /// </summary>
    public class LeaveGroupRequest
    {
        /// <summary>
        /// Identificador del grupo.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Group
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación SearchChatMessagesByGroup.
    /// </summary>
    public class SearchChatMessagesByGroupRequest
    {
        /// <summary>
        /// Identificador del grupo.
        /// </summary>
        public int GroupId { get; set; }
    }
}

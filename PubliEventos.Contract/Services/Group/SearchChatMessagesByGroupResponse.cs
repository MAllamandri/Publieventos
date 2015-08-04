namespace PubliEventos.Contract.Services.Group
{
    using PubliEventos.Contract.Class;
    using System.Collections.Generic;

    /// <summary>
    /// Salidas de la operación SearchChatMessagesByGroup.
    /// </summary>
    public class SearchChatMessagesByGroupResponse
    {
        /// <summary>
        /// Mensajes del grupo.
        /// </summary>
        public List<ChatMessage> Messages { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Group
{
    using PubliEventos.Contract.Class;

    /// <summary>
    /// Salidas de la operación CreateChatMessage.
    /// </summary>
    public class CreateChatMessageResponse
    {
        /// <summary>
        /// Identificador del mensaje.
        /// </summary>
        public ChatMessage Message { get; set; }
    }
}

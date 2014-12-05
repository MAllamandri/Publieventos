namespace PubliEventos.Contract.Services.Comment
{
    using PubliEventos.Contract.Class;
    using System.Collections.Generic;

    /// <summary>
    /// Respuesta del servicio GetCommentsByEvent.
    /// </summary>
    public class GetCommentsByEventResponse
    {
        public List<Comment> Comments { get; set; }
    }
}

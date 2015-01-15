namespace PubliEventos.Contract.Services.Group
{
    using Contract.Class;

    /// <summary>
    /// Salidas de la operación GetGroupById.
    /// </summary>
    public class GetGroupByIdResponse
    {
        /// <summary>
        /// Grupo.
        /// </summary>
        public Group Group { get; set; }
    }
}

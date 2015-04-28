namespace PubliEventos.Contract.Services.Group
{
    /// <summary>
    /// Parametros de la operación SearchGroupsByPartialName.
    /// </summary>
    public class SearchGroupsByPartialNameRequest
    {
        /// <summary>
        /// Nombre de grupo.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Página.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Tamaño de página.
        /// </summary>
        public int PageSize { get; set; }
    }
}

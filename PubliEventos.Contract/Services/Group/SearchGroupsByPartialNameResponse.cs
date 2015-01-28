namespace PubliEventos.Contract.Services.Group
{
    using Contract.Class;
    using System.Collections.Generic;

    /// <summary>
    /// Salidas de la operación SearchGroupsByPartialName.
    /// </summary>
    public class SearchGroupsByPartialNameResponse
    {
        /// <summary>
        /// Usuarios obtenidos en la búsqueda.
        /// </summary>
        public List<Group> Groups { get; set; }

        /// <summary>
        /// Cantidad de resultados encontrados.
        /// </summary>
        public int Quantity { get; set; }
    }
}

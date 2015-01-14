namespace PubliEventos.Contract.Services.Account
{
    using PubliEventos.Contract.Class;
    using System.Collections.Generic;

    /// <summary>
    /// Salidas de la operación SearchUsersByPartialUserName.
    /// </summary>
    public class SearchUsersByPartialUserNameResponse
    {
        /// <summary>
        /// Usuarios obtenidos en la búsqueda.
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// Cantidad de resultados encontrados.
        /// </summary>
        public int Quantity { get; set; }
    }
}

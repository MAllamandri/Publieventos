namespace PubliEventos.Contract.Services.Account
{
    /// <summary>
    /// Parametros de la operación SearchUsersByPartialUserName.
    /// </summary>
    public class SearchUsersByPartialUserNameRequest
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        public string UserName { get; set; }

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

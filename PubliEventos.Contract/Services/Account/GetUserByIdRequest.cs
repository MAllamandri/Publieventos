namespace PubliEventos.Contract.Services.Account
{
    /// <summary>
    /// Representa los parámetros de entrada de la operación GetUserById.
    /// </summary>
    public class GetUserByIdRequest
    {
        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public int UserId { get; set; }
    }
}

namespace PubliEventos.Contract.Services.UsersGroup
{
    using System.Collections.Generic;
    using PubliEventos.Contract.Class;

    /// <summary>
    /// Representa la salida de la operación GetUsersGroupByUser.
    /// </summary>
    public class GetUsersGroupByUserResponse
    {
        /// <summary>
        /// Grupos del usuario.
        /// </summary>
        public List<Group> Groups { get; set; }
    }
}

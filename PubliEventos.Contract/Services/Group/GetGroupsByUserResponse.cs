namespace PubliEventos.Contract.Services.Group
{
    using System.Collections.Generic;
    using PubliEventos.Contract.Class;

    /// <summary>
    /// Representa la salida de la operación GetUsersGroupByUser.
    /// </summary>
    public class GetGroupsByUserResponse
    {
        /// <summary>
        /// Grupos del usuario.
        /// </summary>
        public List<Group> Groups { get; set; }
    }
}

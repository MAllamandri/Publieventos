namespace PubliEventos.Contract.Services.Group
{
    using System.Collections.Generic;

    /// <summary>
    /// Resultados de la operación EditGroup.
    /// </summary>
    public class EditGroupResponse
    {
        /// <summary>
        /// Grupo que se edito.
        /// </summary>
        public List<int> UserIdsToSendInvitation { get; set; }
    }
}

namespace PubliEventos.Contract.Services.Group
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Representa un los paramtros de entrada de la operación CreateGroup.
    /// </summary>
    public class EditGroupRequest : GroupBaseRequest
    {
        /// <summary>
        /// Identificador del grupo.
        /// </summary>
        [Required]
        public int GroupId { get; set; }

        /// <summary>
        /// Nombre de usuarios separados por coma.
        /// </summary>
        public string UserNames { get; set; }
    }
}

namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;

    /// <summary>
    /// Representa la asociación de un usuario a un grupo.
    /// </summary>
    public class UsersGroup : BaseIdentifier<int>
    {
        /// <summary>
        /// Identificador del Grupo.
        /// </summary>
        public virtual int GroupId { get; set; }

        /// <summary>
        /// Identificador del usuario.
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// Indica si el usuario esta activo en el grupo.
        /// </summary>
        public virtual bool? Active { get; set; }

        /// <summary>
        /// Fecha de alta.
        /// </summary>
        public virtual DateTime EffectDate { get; set; }

        /// <summary>
        /// Fecha de eliminación.
        /// </summary>
        public virtual DateTime? NullDate { get; set; }
    }
}

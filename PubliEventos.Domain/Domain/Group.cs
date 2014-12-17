namespace PubliEventos.Domain.Domain
{
    using PubliEventos.DataAccess.Infrastructure;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Representa un grupo de usuarios.
    /// </summary>
    public class Group : BaseIdentifier<int>
    {
        /// <summary>
        /// Nombre del grupo.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Usuario administrador.
        /// </summary>
        public virtual User Administrator { get; set; }

        /// <summary>
        /// Usuarios del grupo.
        /// </summary>
        public virtual IList<User> Users { get; set; }

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

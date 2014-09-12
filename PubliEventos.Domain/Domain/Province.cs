namespace PubliEventos.Domain.Domain
{
    using System.Collections.Generic;
    using PubliEventos.DataAccess.Infrastructure;

    public class Province : BaseIdentifier<int>
    {
        /// <summary>
        /// Nombre de la provincia.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Localidades en la provincia.
        /// </summary>
        public virtual IList<Locality> Localities { get; set; }
    }
}

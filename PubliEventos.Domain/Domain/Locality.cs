namespace PubliEventos.Domain.Domain
{
    using System;
    using PubliEventos.DataAccess.Infrastructure;

    /// <summary>
    /// Clase localidad.
    /// </summary>
    public class Locality : BaseIdentifier<int>
    {
        /// <summary>
        /// Nombre de la localidad.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Provincia de la localidad.
        /// </summary>
        public virtual string Province { get; set; }

        /// <summary>
        /// Latitud de la localidad.
        /// </summary>
        public virtual float Latitude { get; set; }

        /// <summary>
        /// Longitud de la localidad.
        /// </summary>
        public virtual float Longitude { get; set; }
    }
}

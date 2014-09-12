namespace PubliEventos.Services
{
    using System.Collections.Generic;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Domain.Domain;
    using PubliEventos.Services.Services;

    /// <summary>
    /// Manejador de servicios.
    /// </summary>
    public class ServicesLocalitiesHandler : IServiceLocalities
    {
        /// <summary>
        /// Obtiene todas las localidades.
        /// </summary>
        /// <returns>Lista de localidades.</returns>
        public List<Locality> GetAllLocalities()
        {
            return new ServiceLocalities().GetAllLocalities();
        }

        /// <summary>
        /// Obtiene todas las provincias.
        /// </summary>
        /// <returns>Lista de provincias.</returns>
        public List<Province> GetAllProvinces()
        {
            return new ServiceLocalities().GetAllProvinces();
        }
    }
}

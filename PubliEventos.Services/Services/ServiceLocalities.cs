namespace PubliEventos.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using PubliEventos.DataAccess.Querys;
    using PubliEventos.Domain.Domain;

    /// <summary>
    /// Servicio de localidades.
    /// </summary>
    public class ServiceLocalities
    {
        /// <summary>
        /// Obtiene todas las localidades.
        /// </summary>
        /// <returns>Lista de localidades.</returns>
        public List<Locality> GetAllLocalities()
        {
            return new BaseQuery<Locality, int>().LoadAll().ToList();
        }

        /// <summary>
        /// Obtiene todas las provincias.
        /// </summary>
        /// <returns>Lista de provincias.</returns>
        public List<Province> GetAllProvinces()
        {
            return new BaseQuery<Province, int>().LoadAll().ToList();
        }
    }
}

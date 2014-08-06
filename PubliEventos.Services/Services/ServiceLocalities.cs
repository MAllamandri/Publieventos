namespace PubliEventos.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PubliEventos.DataAccessNH.Querys;
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
        public static List<Locality> GetAllLocalities()
        {
            return new BaseQuery<Locality, Int64>().LoadAll().ToList();
        }
    }
}

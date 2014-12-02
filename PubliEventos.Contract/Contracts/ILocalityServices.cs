namespace PubliEventos.Contract.Contracts
{
    using System.Collections.Generic;
    using PubliEventos.Domain.Domain;

    /// <summary>
    /// Interface del servicio de localidades.
    /// </summary>
    public interface ILocalityServices
    {
        /// <summary>
        /// Obtiene todas las localidades.
        /// </summary>
        /// <returns>Lista de localidades.</returns>
        List<Locality> GetAllLocalities();

        /// <summary>
        /// Obtiene todas las provincias.
        /// </summary>
        /// <returns></returns>
        List<Province> GetAllProvinces();
    }
}

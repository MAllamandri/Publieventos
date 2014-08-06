namespace PubliEventos.Contract.Contracts
{
    using System.Collections.Generic;
    using PubliEventos.Domain.Domain;

    /// <summary>
    /// Interface del servicio de localidades.
    /// </summary>
    public interface IServiceLocalities
    {
        /// <summary>
        /// Obtiene todas las localidades.
        /// </summary>
        /// <returns>Lista de localidades.</returns>
        List<Locality> GetAllLocalities();
    }
}

namespace PubliEventos.Services
{
    using System.Collections.Generic;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Domain.Domain;
    using PubliEventos.Services.Services;

    /// <summary>
    /// Manejador de servicios.
    /// </summary>
    public class ServicesHandler : IServiceLocalities
    {
        /// <summary>
        /// Obtiene todas las localidades.
        /// </summary>
        /// <returns>Lista de localidades.</returns>
        public List<Locality> GetAllLocalities()
        {
            return ServiceLocalities.GetAllLocalities();
        }
    }
}

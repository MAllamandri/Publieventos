namespace PubliEventos.Contract.Contracts
{
    using System.Collections.Generic;
    using PubliEventos.Domain.Domain;

    /// <summary>
    /// Interface del servicio de eventos.
    /// </summary>
    public interface IServiceEvents
    {
        /// <summary>
        /// Obtengo todos los usuarios.
        /// </summary>
        /// <returns>Lista de usuarios.</returns>
        List<Event> GetAllEvents();

        List<MultimediaContent> All();
    }
}

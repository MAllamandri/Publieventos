namespace PubliEventos.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using PubliEventos.DataAccess.Querys;
    using PubliEventos.Domain.Domain;

    public class ServiceEvents
    {
        /// <summary>
        /// Obtiene todos los eventos.
        /// </summary>
        /// <returns>Lista de eventos.</returns>
        public static List<Event> GatAllEvents()
        {
            return new BaseQuery<Event, int>().LoadAll().ToList();
        }

        public static List<MultimediaContent> All()
        {
            return new BaseQuery<MultimediaContent, int>().LoadAll().ToList();
        }
    }
}

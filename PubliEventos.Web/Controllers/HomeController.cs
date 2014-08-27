namespace PubliEventos.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Web.Models;

    public class HomeController : Controller
    {
        /// <summary>
        /// Servicio de eventos.
        /// </summary>
        [Dependency]
        public IServiceEvents serviceEvents { get; set; }

        /// <summary>
        /// Vista index.
        /// </summary>
        /// <returns>Index view.</returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            var events = new EventsModel();
            events.events = serviceEvents.GetAllEvents().OrderBy(x => x.EventDate).ToList();

            return View(events);
        }
    }
}

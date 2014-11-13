namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.ServicesEvents;
    using System;
    using System.Linq;
    using System.Web.Mvc;

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
            ViewBag.events = serviceEvents.SearchFilteredEvents(new SearchFilteredEventsRequest() { StartDate = DateTime.Now.AddMonths(-3), EndDate = DateTime.Now.AddMonths(6) })
                .GroupBy(x => x.EventDate)
                .Select(grp => grp.ToList())
                .OrderByDescending(group => group.First().EventDate)
                .ToList();

            return View();
        }
    }
}

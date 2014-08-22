using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using PubliEventos.Contract.Contracts;
using PubliEventos.Web.Models;

namespace PubliEventos.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Servicio de eventos.
        /// </summary>
        [Dependency]
        public IServiceEvents serviceEvents { get; set; }

        public ActionResult Index()
        {
            var events = new EventsModel();
            events.events = serviceEvents.GetAllEvents().OrderBy(x=> x.EventDate).ToList();

            return View(events);
        }

    }
}

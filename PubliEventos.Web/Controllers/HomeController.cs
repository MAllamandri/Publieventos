using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using PubliEventos.Contract.Class;

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
            ViewBag.events = serviceEvents.GetAllEvents()
                .Where(x => x.EventDate >= DateTime.Now.AddMonths(-3) && x.EventDate <= DateTime.Now.AddMonths(6))
                .GroupBy(x => x.EventDate)
                .ToList();


            return View();
        }
    }
}

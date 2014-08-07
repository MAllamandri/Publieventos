namespace PubliEventos.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;

    /// <summary>
    /// Controlador de cuentas.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IServiceLocalities serviceLocalities { get; set; }

        /// <summary>
        /// Servicio de eventos.
        /// </summary>
        [Dependency]
        public IServiceEvents serviceEvents { get; set; }

        /// <summary>
        /// Vista de login.
        /// </summary>
        /// <returns>Login view.</returns>
        public ActionResult Login()
        {
            var events = serviceEvents.GetAllEvents();

            var contents = serviceEvents.All();

            var id = (int)contents[0].ContentType;
            events[0].Description = "Malisima la peli.";
            var localities = serviceLocalities.GetAllLocalities();
            return View();
        }
    }
}

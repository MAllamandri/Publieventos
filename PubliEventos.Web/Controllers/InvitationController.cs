namespace PubliEventos.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de invitaciones.
    /// </summary>
    [Authorize]
    public class InvitationController : BaseController
    {
        /// <summary>
        /// Vista de mis invitaciones.
        /// </summary>
        /// <returns></returns>
        public ActionResult MyInvitations()
        {
            return View();
        }
    }
}

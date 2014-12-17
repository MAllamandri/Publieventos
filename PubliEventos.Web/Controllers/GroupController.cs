namespace PubliEventos.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de grupos.
    /// </summary>
    [Authorize]
    public class GroupController : BaseController
    {
        /// <summary>
        /// Vista de los grupos de un usuario.
        /// </summary>
        /// <returns></returns>
        public ActionResult MyGroups()
        {
            return View();
        }
    }
}

namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Group;
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
        #region Properties

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IGroupServices serviceGroup { get; set; }

        #endregion

        /// <summary>
        /// Vista de los grupos de un usuario.
        /// </summary>
        /// <returns></returns>
        public ActionResult MyGroups()
        {
            ViewBag.groups = this.serviceGroup.GetGroupsByUser(new GetGroupsByUserRequest() { UserId = User.Id }).Groups;

            return View();
        }
    }
}

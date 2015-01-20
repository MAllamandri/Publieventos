namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Invitation;
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
        #region Properties

        /// <summary>
        /// Servicio de invitaciones.
        /// </summary>
        [Dependency]
        public IInvitationServices servicesInvitations { get; set; }

        #endregion

        #region Views

        /// <summary>
        /// Vista de mis invitaciones.
        /// </summary>
        /// <returns></returns>
        public ActionResult MyInvitations()
        {
            ViewBag.invitations = this.servicesInvitations.SearchInvitationsByUser(new SearchInvitationsByUserRequest() { UserId = User.Id }).Invitations;

            return View();
        }

        #endregion
    }
}

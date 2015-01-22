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

        #region Json Methods

        /// <summary>
        /// Registra la respuesta a una invitación.
        /// </summary>
        /// <param name="invitationId">Identificador de la invitación.</param>
        /// <param name="reply">Respuesta a la invitación.</param>
        /// <returns>True si se registro correctamente, false caso contrario.</returns>
        public JsonResult ReplyInvitation(int invitationId, bool reply)
        {
            try
            {
                this.servicesInvitations.ReplyInvitation(new ReplyInvitationRequest() { InvitationId = invitationId, Reply = reply });

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion
    }
}

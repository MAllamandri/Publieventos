namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Group;
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

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IEventServices serviceEvents { get; set; }

        /// <summary>
        /// Servicio de grupos.
        /// </summary>
        [Dependency]
        public IGroupServices serviceGroups { get; set; }

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

        /// <summary>
        /// Vista de invitación a evento.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>InviteToEvent view.</returns>
        public ActionResult InviteToEvent(int id)
        {
            var model = this.serviceEvents.GetEventById(id);

            return View(model);
        }

        #endregion

        #region Json Methods

        /// <summary>
        /// Invita a usuarios y grupos a un evento.
        /// </summary>
        /// <param name="usersIds"></param>
        /// <param name="groupsIds"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InviteToEvent(string usersIds, string groupsIds, int eventId)
        {
            if (!string.IsNullOrEmpty(usersIds) || !string.IsNullOrEmpty(groupsIds))
            {
                var _event = this.serviceEvents.GetEventById(eventId);

                // Busco las invitaciones del evento para verificar de que el usuario no tenga una invitación activa.
                var invitations = this.servicesInvitations.SearchInvitationsByEvent(new SearchInvitationsByEventRequest() { EventId = _event.Id.Value }).Invitations;

                #region Users

                var users = !string.IsNullOrEmpty(usersIds) ? usersIds.Split(',') : new string[0];

                if (users != null && users.Count() > 0)
                {
                    //Mando las invitaciones.
                    foreach (var user in users)
                    {
                        if (_event.User.Id != Convert.ToInt32(user) && !invitations.Where(x => x.User.Id == Convert.ToInt32(user) && !x.Confirmed.HasValue).Any())
                        {
                            this.servicesInvitations.CreateInvitation(new CreateInvitationRequest()
                            {
                                EventId = _event.Id,
                                UserId = Convert.ToInt32(user)
                            });
                        }
                    }
                }

                #endregion

                #region Groups

                if (!string.IsNullOrEmpty(groupsIds))
                {
                    //Mando las invitaciones a los miembtros del grupo.
                    foreach (var groupId in groupsIds.Split(','))
                    {
                        var group = this.serviceGroups.GetGroupById(new GetGroupByIdRequest() { GroupId = Convert.ToInt32(groupId) }).Group;

                        foreach (var userId in group.UsersGroup.Select(x => x.UserId))
                        {
                            // Si ya se le mando la invitación, o es el administrador o tiene una invitación al evento pendiente, no le envio.
                            if (!users.Contains(userId.ToString()) && _event.User.Id != Convert.ToInt32(userId) && !invitations.Where(x => x.User.Id == userId && !x.Confirmed.HasValue).Any())
                            {
                                this.servicesInvitations.CreateInvitation(new CreateInvitationRequest()
                                {
                                    EventId = _event.Id,
                                    UserId = Convert.ToInt32(userId)
                                });
                            }
                        }
                    }
                }

                #endregion

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false, Errors = "Seleccione usuarios a quien enviar la invitación." }, JsonRequestBehavior.AllowGet);
        }

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

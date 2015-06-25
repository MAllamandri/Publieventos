namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Enums;
    using PubliEventos.Contract.Services.Group;
    using PubliEventos.Contract.Services.Invitation;
    using PubliEventos.Web.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        [UserActionRestriction(ValidateCondition.InvitationToEvent)]
        public ActionResult InviteToEvent(int id)
        {
            var model = this.serviceEvents.GetEventById(id);

            var invitations = this.servicesInvitations.SearchInvitationsByEvent(new SearchInvitationsByEventRequest() { EventId = id }).Invitations;

            ViewBag.participants = invitations.Where(x => x.Confirmed == true).Select(x => x.User).ToList();
            ViewBag.standby = invitations.Where(x => !x.Confirmed.HasValue).Select(x => x.User).ToList();

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
                var invitations = this.servicesInvitations.SearchInvitationsByEvent(new SearchInvitationsByEventRequest()
                {
                    EventId = _event.Id.Value
                }).Invitations;

                var ids = new List<int>();

                if (!string.IsNullOrEmpty(usersIds))
                {
                    var usersToInvite = usersIds.Split(',');

                    foreach (var id in usersToInvite)
                    {
                        var lastInvitation = invitations.Where(y => y.User.Id == Convert.ToInt32(id)).OrderByDescending(y => y.EffectDate).FirstOrDefault();

                        if (Convert.ToInt32(id) != _event.User.Id && (lastInvitation == null || lastInvitation.Confirmed.HasValue && !lastInvitation.Confirmed.Value))
                        {
                            ids.Add(Convert.ToInt32(id));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(groupsIds))
                {
                    //Mando las invitaciones a los miembtros del grupo.
                    foreach (var groupId in groupsIds.Split(','))
                    {
                        var group = this.serviceGroups.GetGroupById(new GetGroupByIdRequest() { GroupId = Convert.ToInt32(groupId) }).Group;

                        foreach (var id in group.UsersGroup.Select(x => x.UserId))
                        {
                            var lastInvitation = invitations.Where(y => y.User.Id == id).OrderByDescending(y => y.EffectDate).FirstOrDefault();

                            if (!ids.Contains(id) && _event.User.Id != id &&
                                (lastInvitation == null || lastInvitation.Confirmed.HasValue && !lastInvitation.Confirmed.Value))
                            {
                                ids.Add(id);
                            }
                        }
                    }
                }

                if (ids.Any())
                {
                    this.servicesInvitations.CreateInvitation(new CreateInvitationRequest()
                    {
                        EventId = eventId,
                        UserIds = ids
                    });
                }

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

        /// <summary>
        /// Marca asistencia del usuario logueado al evento.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>True.</returns>
        [HttpPost]
        public JsonResult AttendEvent(int eventId)
        {
            this.servicesInvitations.AttendEvent(new AttendEventRequest { EventId = eventId, UserId = User.Id });

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

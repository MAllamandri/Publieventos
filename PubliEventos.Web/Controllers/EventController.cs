namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Enums;
    using PubliEventos.Contract.Services.Comment;
    using PubliEventos.Contract.Services.Event;
    using PubliEventos.Contract.Services.Invitation;
    using PubliEventos.Web.Helpers;
    using PubliEventos.Web.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    [Authorize]
    public class EventController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de eventos.
        /// </summary>
        [Dependency]
        public IEventServices serviceEvents { get; set; }

        /// <summary>
        /// Servicio de comentarios.
        /// </summary>
        [Dependency]
        public ICommentServices serviceComments { get; set; }

        /// <summary>
        /// Servicio de invitaciones.
        /// </summary>
        [Dependency]
        public IInvitationServices servicesInvitations { get; set; }

        #endregion

        /// <summary>
        /// Vista de creación de eventos.
        /// </summary>
        /// <returns>Create View.</returns>
        public ActionResult Create()
        {
            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description");

            return View();
        }

        /// <summary>
        /// Vista de edición de eventos.
        /// </summary>
        /// <returns>Edit view.</returns>
        [UserActionRestriction(ElementTypesToValidate.Event)]
        public ActionResult Edit(int id)
        {
            var model = this.GetEventSummary(this.serviceEvents.GetEventById(id));

            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description");

            return View(model);
        }

        /// <summary>
        /// Vista detalle de eventos.
        /// </summary>
        /// <param name="id">Id del evento.</param>
        /// <returns>Detail view.</returns>
        [UserActionRestriction(ElementTypesToValidate.Event)]
        public ActionResult Detail(int id)
        {
            var model = this.serviceEvents.GetEventById(id);

            if (model != null)
            {
                var invitations = this.servicesInvitations.SearchInvitationsByEvent(new SearchInvitationsByEventRequest() { EventId = id }).Invitations;

                ViewBag.participants = invitations.Where(x => x.Confirmed == true).Select(x => x.User).ToList();
                ViewBag.standby = invitations.Where(x => !x.Confirmed.HasValue).Select(x => x.User).ToList();
                ViewBag.comments = this.serviceComments.GetCommentsByEvent(new GetCommentsByEventRequest() { EventId = id }).Comments.OrderByDescending(x => x.EffectDate).ToList();
            }

            return View(model);
        }

        /// <summary>
        /// Vista mis eventos.
        /// </summary>
        /// <param name="currentEvents">Indica si se obtienen los eventos actuales o los que ya pasaron.</param>
        /// <returns>MyEvents view.</returns>
        public ActionResult MyEvents(bool currentEvents)
        {
            List<Event> events = new List<Event>();

            if (currentEvents)
            {
                events = this.serviceEvents.GetAllEvents().Where(x => x.User.Id == User.Id && x.EventDate >= DateTime.Now.Date).ToList();
            }
            else
            {
                events = this.serviceEvents.GetAllEvents().Where(x => x.User.Id == User.Id && x.EventDate < DateTime.Now.Date).ToList();
            }

            ViewBag.myEvents = events;
            ViewBag.currentsEvents = currentEvents;
            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description");

            return View();
        }

        public ActionResult UploadPictures(int id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadPictures()
        {
            bool isSavedSuccessfully = true;

            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                //Save file content goes here
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = "File saved" });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Vista parcial de eventos.
        /// </summary>
        /// <param name="model">modelo de filtros.</param>
        /// <returns>Mosaic view.</returns>
        [HttpPost]
        public PartialViewResult GetFilteredEvents(SearchFilteredEventsRequest model, bool myEvents)
        {
            // filtro por mis eventos.
            model.IdUser = myEvents ? User.Id : (int?)null;

            var events = this.serviceEvents.SearchFilteredEvents(model);

            return PartialView("Partial/_Mosaic", events);
        }


        #region Private Methods

        /// <summary>
        /// Parseo el evento al modelo de edición o creación.
        /// </summary>
        /// <param name="eventToParse">Evento a parsear.</param>
        /// <returns>Modelo de la vista.</returns>
        private EventCreateOrUpdateRequest GetEventSummary(Event eventToParse)
        {
            return new EventCreateOrUpdateRequest()
            {
                Id = eventToParse.Id,
                Title = eventToParse.Title,
                Detail = eventToParse.Detail,
                Active = eventToParse.Active,
                FileName = eventToParse.FileName,
                Description = eventToParse.Description,
                EventDate = eventToParse.EventDate.Date,
                EventStartTime = eventToParse.EventStartTime,
                EventEndTime = eventToParse.EventEndTime,
                Private = eventToParse.Private,
                UserId = eventToParse.User.Id.Value,
                EventTypeId = eventToParse.EventType.Id.Value,
                Latitude = eventToParse.Latitude.ToString(),
                Longitude = eventToParse.Longitude.ToString()
            };
        }

        #endregion

        #region Json Methods

        /// <summary>
        /// Vista de creación de eventos.
        /// </summary>
        /// <param name="model">Modelo de la vista.</param>
        /// <returns>Create View.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(EventCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid)
            {
                // Seteo el usuario creador.
                model.UserId = User.Id;

                if (model.CoverPhoto != null)
                {
                    // Renombro el archivo.
                    var fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(model.CoverPhoto.FileName), DateTime.Now.ToString("ddMMyyyyhhMMss"), Path.GetExtension(model.CoverPhoto.FileName));
                    model.FileName = fileName;
                    var path = Path.Combine(HttpContext.Server.MapPath(pathCoverPhoto), Path.GetFileName(fileName));

                    model.CoverPhoto.SaveAs(path);
                }

                // Doy de alta el evento.
                this.serviceEvents.CreateEvent(model);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false, Errors = ModelErrors.GetModelErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Vista de edición de eventos.
        /// </summary>
        /// <returns>Edit view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(EventCreateOrUpdateRequest model)
        {
            if (ModelState.IsValid)
            {
                if (model.CoverPhoto != null)
                {
                    if (!string.IsNullOrEmpty(model.FileName))
                    {
                        // Elimino la portada anterior.
                        System.IO.File.Delete(HttpContext.Server.MapPath(pathCoverPhoto + model.FileName));
                    }

                    var fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(model.CoverPhoto.FileName), DateTime.Now.ToString("ddMMyyyyhhMMss"), Path.GetExtension(model.CoverPhoto.FileName));
                    model.FileName = fileName;
                    var path = Path.Combine(HttpContext.Server.MapPath(pathCoverPhoto), Path.GetFileName(fileName));

                    model.CoverPhoto.SaveAs(path);
                }

                // Edito el evento.
                this.serviceEvents.EditEvent(model);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false, Errors = ModelErrors.GetModelErrors(ModelState) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Elimina un evento.
        /// </summary>
        /// <param name="idEvent">Identificador del evento.</param>
        /// <returns>True si se realizo correctamente, false caso contrario.</returns>
        public JsonResult DeleteEvent(int idEvent)
        {
            try
            {
                // Elimino el evento.
                this.serviceEvents.DeleteEvent(idEvent);

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

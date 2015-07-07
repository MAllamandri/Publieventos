namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Enums;
    using PubliEventos.Contract.Services.Event;
    using PubliEventos.Contract.Services.Invitation;
    using PubliEventos.Web.Helpers;
    using PubliEventos.Web.Models.EventModels;
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

        #region Views

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
        [UserActionRestriction(ValidateCondition.Event)]
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
        [UserActionRestriction(ValidateCondition.Event)]
        public ActionResult Detail(int id)
        {
            var model = this.serviceEvents.GetEventById(id);
            var invitations = this.servicesInvitations.SearchInvitationsByEvent(new SearchInvitationsByEventRequest() { EventId = id }).Invitations;

            ViewBag.participants = invitations.Where(x => x.Confirmed == true).Select(x => x.User).ToList();
            ViewBag.standby = invitations.Where(x => !x.Confirmed.HasValue).Select(x => x.User).ToList();

            // Ultima vez que el usuario marco asistencia.
            var lastInvitation = invitations.Where(x => x.User.Id == User.Id).OrderByDescending(x => x.EffectDate).FirstOrDefault();

            ViewBag.AttendEventByCurrentUser = lastInvitation != null && lastInvitation.Confirmed.HasValue && lastInvitation.Confirmed.Value ? true : false;

            // Distingo entre fotos y videos.
            ViewBag.pictures = model.MultimediaContents != null && model.MultimediaContents.Any() ? model.MultimediaContents.Where(x => x.ContentType == (int)ContentTypes.Image).Select(x => new MultimediaContentSummaryModel()
            {
                FileName = x.FileName,
                IsReportedByUser = x.Reports != null && x.Reports.Where(r => !r.IsReported.HasValue).Select(r => r.User.Id.Value).ToList().Contains(User.Id) ? true : false,
                ContentType = (int)ContentTypes.Image
            }).ToList() : null;

            ViewBag.movies = model.MultimediaContents != null && model.MultimediaContents.Any() ? model.MultimediaContents.Where(x => x.ContentType == (int)ContentTypes.Movie).Select(x => new MultimediaContentSummaryModel()
            {
                FileName = x.FileName,
                IsReportedByUser = x.Reports != null && x.Reports.Where(r => !r.IsReported.HasValue).Select(r => r.User.Id.Value).ToList().Contains(User.Id) ? true : false,
                ContentType = (int)ContentTypes.Movie
            }).ToList() : null;

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
                events = this.serviceEvents.SearchFilteredEvents(new SearchFilteredEventsRequest()
                {
                    UserId = User.Id,
                    StartDate = DateTime.Now.Date
                }).Events
                    .OrderByDescending(x => x.EventDate)
                    .ToList();
            }
            else
            {
                events = this.serviceEvents.SearchFilteredEvents(new SearchFilteredEventsRequest()
                {
                    UserId = User.Id,
                    EndDate = DateTime.Now.Date.AddDays(-1)
                }).Events
                    .OrderByDescending(x => x.EventDate)
                    .ToList();
            }

            ViewBag.myEvents = events;
            ViewBag.currentsEvents = currentEvents;
            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description");

            return View();
        }

        /// <summary>
        /// Vista de alta de contenidos multimedia.
        /// </summary>
        /// <param name="id">Identificador del evento.</param>
        /// <returns>UploadPictures view.</returns>
        [UserActionRestriction(ValidateCondition.UploadPictures)]
        public ActionResult UploadPictures(int id)
        {
            ViewBag.eventId = id;

            return View();
        }

        /// <summary>
        /// Vista que muestra los eventos a los que asistio el usuario.
        /// </summary>
        /// <returns>AssistsRecord view.</returns>
        public ActionResult AssistsRecord()
        {
            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description");

            var events = this.servicesInvitations.SearchEventsUserConfirmed(new SearchEventsUserConfirmedRequest { UserId = User.Id }).Events;

            ViewBag.futureEvents = events.Any() ? events.Where(x => x.EventDate.Date >= DateTime.Now).ToList() : events;
            ViewBag.previousEvents = events.Any() ? events.Where(x => x.EventDate.Date < DateTime.Now).ToList() : events;

            return View();
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
            model.UserId = myEvents ? User.Id : (int?)null;

            var events = this.serviceEvents.SearchFilteredEvents(model).Events;
            ViewData["ViewActions"] = true;

            return PartialView("Partial/_Mosaic", events);
        }

        /// <summary>
        /// Obtiene los eventos a los que asistio el usuario.
        /// </summary>
        /// <param name="model">SearchEventsUserConfirmedRequest model.</param>
        /// <returns>_Mosaic partial.</returns>
        [HttpPost]
        public PartialViewResult SearchEventsUserConfirmed(SearchEventsUserConfirmedRequest model)
        {
            model.UserId = User.Id;

            var events = this.servicesInvitations.SearchEventsUserConfirmed(model).Events;
            ViewData["ViewActions"] = false;

            return PartialView("Partial/_Mosaic", events);
        }

        #endregion

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

        /// <summary>
        /// Elimina un contenido multimedia.
        /// </summary>
        /// <param name="fileName">Identificador del archivo.</param>
        /// <param name="eventId">Identificador del evento.</param>
        private void DeleteMultimediaContent(string fileName, int eventId, int contentType)
        {
            if (System.IO.File.Exists(pathEventsPictures + fileName) && contentType == (int)ContentTypes.Image)
            {
                //Libero y Elimino el contenido.
                GC.Collect();
                GC.WaitForPendingFinalizers();
                FileInfo file = new FileInfo(pathEventsPictures + fileName);
                var stream = file.OpenRead();
                stream.Close();
                stream.Dispose();
                file.Delete();
            }

            this.serviceEvents.DeleteMultimediaContent(new DeleteMultimediaContentRequest() { EventId = eventId, FileName = fileName });
        }

        #endregion

        #region Json Methods

        /// <summary>
        /// Da de alta contenido multimedia a un evento.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <returns>True o false, y Id del archivo.</returns>
        [HttpPost]
        public JsonResult UploadPictures(int eventId, int? nullParameter)
        {
            var fileName = string.Empty;
            bool isSavedSuccessfully = true;

            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase fileToSave = Request.Files[file];

                    // Renombro el archivo.
                    fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(fileToSave.FileName), DateTime.Now.ToString("ddMMyyyyhhMMssfff"), Path.GetExtension(fileToSave.FileName));

                    var path = Path.Combine(pathEventsPictures, Path.GetFileName(fileName));

                    fileToSave.SaveAs(path);

                    // Doy de alta el contenido al evento.
                    this.serviceEvents.CreateMultimediaContent(new CreateMultimediaContentRequest()
                    {
                        EventId = eventId,
                        FileName = fileName,
                        ContentType = (int)ContentTypes.Image
                    });
                }
            }
            catch (Exception e)
            {
                isSavedSuccessfully = false;

                this.DeleteMultimediaContent(fileName, eventId, (int)ContentTypes.Image);
            }

            return Json(new { Success = isSavedSuccessfully, FileName = fileName }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sube un video.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <param name="fileName">Identificador del video.</param>
        /// <returns>True si subio correctamente, false caso contrario.</returns>
        [HttpPost]
        public JsonResult UploadMovies(int eventId, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                // Doy de alta el contenido al evento.
                this.serviceEvents.CreateMultimediaContent(new CreateMultimediaContentRequest()
                {
                    EventId = eventId,
                    FileName = fileName,
                    ContentType = (int)ContentTypes.Movie
                });

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Elimina un archivo asociado a un evento.
        /// </summary>
        /// <param name="fileName">Nombre del archivo (único).</param>
        /// <returns></returns>
        public JsonResult DeleteContent(string fileName, int eventId, int contentType)
        {
            try
            {
                this.DeleteMultimediaContent(fileName, eventId, contentType);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }

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
                    var path = Path.Combine(pathCoverPhoto, Path.GetFileName(fileName));

                    model.CoverPhoto.SaveAs(path);
                }

                // Doy de alta el evento.
                var eventId = this.serviceEvents.CreateEvent(model).EventId;

                return Json(new { Success = true, EventId = eventId }, JsonRequestBehavior.AllowGet);
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
                        System.IO.File.Delete(pathCoverPhoto + model.FileName);
                    }

                    var fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(model.CoverPhoto.FileName), DateTime.Now.ToString("ddMMyyyyhhMMss"), Path.GetExtension(model.CoverPhoto.FileName));
                    model.FileName = fileName;
                    var path = Path.Combine(pathCoverPhoto, Path.GetFileName(fileName));

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

        /// <summary>
        /// Valida si el contenido ya existe para un evento en particular.
        /// </summary>
        /// <param name="eventId">Identificador del evento.</param>
        /// <param name="fileName">Identificador del archivo.</param>
        /// <param name="contentType">Tipo de contenido.</param>
        /// <returns>True si existe, false caso contrario.</returns>
        public JsonResult ValidateExistsContent(int eventId, string fileName, int contentType)
        {
            var exists = this.serviceEvents.ValidateExistsContent(new ValidateExistsContentRequest
            {
                EventId = eventId,
                FileName = fileName,
                ContentType = contentType
            }).Exists;

            return Json(new { Exists = exists }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

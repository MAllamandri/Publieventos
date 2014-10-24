﻿namespace PubliEventos.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Class;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.ServicesEvents;
    using System.Collections.Generic;
    using PubliEventos.Web.Helpers;

    [Authorize]
    public class EventController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IServiceLocalities ServiceLocalities { get; set; }

        /// <summary>
        /// Servicio de localidades.
        /// </summary>
        [Dependency]
        public IServiceEvents serviceEvents { get; set; }

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
        /// Vista de creación de eventos.
        /// </summary>
        /// <param name="model">Modelo de la vista.</param>
        /// <returns>Create View.</returns>
        [HttpPost]
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
        public ActionResult Edit(int id)
        {
            var model = this.GetEventSummary(this.serviceEvents.GetEventById(id));

            ViewBag.Provinces = new SelectList(ServiceLocalities.GetAllProvinces(), "Id", "Name");
            ViewBag.Localities = new SelectList(ServiceLocalities.GetAllLocalities().Where(x => x.Province.Id == model.ProvinceId).ToList(), "Id", "Name");
            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description");

            return View(model);
        }

        /// <summary>
        /// Vista de edición de eventos.
        /// </summary>
        /// <returns>Edit view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventCreateOrUpdateRequest model)
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

                return RedirectToAction("Index", "Home");
            }

            model.CoverPhoto = null;
            ViewBag.Provinces = new SelectList(ServiceLocalities.GetAllProvinces(), "Id", "Name", model.ProvinceId);
            ViewBag.Localities = new SelectList(ServiceLocalities.GetAllLocalities().Where(x => x.Province.Id == model.ProvinceId).ToList(), "Id", "Name");
            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description", model.EventTypeId);

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
            ViewBag.currentEvents = currentEvents;
            ViewBag.Provinces = new SelectList(ServiceLocalities.GetAllProvinces(), "Id", "Name");
            ViewBag.Localities = new SelectList(ServiceLocalities.GetAllLocalities(), "Id", "Name");
            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description");

            return View();
        }

        /// <summary>
        /// Vista parcial de eventos.
        /// </summary>
        /// <param name="model">modelo de filtros.</param>
        /// <returns>Mosaic view.</returns>
        [HttpPost]
        public PartialViewResult GetFilteredEvents(SearchFilteredEventsRequest model)
        {
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
                //ProvinceId = eventToParse.Locality.Province.Id.Value,
                //LocalityId = eventToParse.Locality.Id.Value,
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

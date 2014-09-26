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
            ViewBag.Provinces = new SelectList(ServiceLocalities.GetAllProvinces(), "Id", "Name");
            ViewBag.Localities = new SelectList(ServiceLocalities.GetAllLocalities(), "Id", "Name");
            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description");

            return View();
        }

        /// <summary>
        /// Vista de creación de eventos.
        /// </summary>
        /// <param name="model">Modelo de la vista.</param>
        /// <returns>Create View.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventCreateOrUpdateParameters model)
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

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Provinces = new SelectList(ServiceLocalities.GetAllProvinces(), "Id", "Name", model.ProvinceId);
            ViewBag.Localities = new SelectList(ServiceLocalities.GetAllLocalities(), "Id", "Name", model.LocalityId);
            ViewBag.EventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description", model.EventTypeId);

            return View();
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
        public ActionResult Edit(EventCreateOrUpdateParameters model)
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

        #region Private Methods

        /// <summary>
        /// Parseo el evento al modelo de edición o creación.
        /// </summary>
        /// <param name="eventToParse">Evento a parsear.</param>
        /// <returns>Modelo de la vista.</returns>
        private EventCreateOrUpdateParameters GetEventSummary(Event eventToParse)
        {
            return new EventCreateOrUpdateParameters()
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
                ProvinceId = eventToParse.Locality.Province.Id.Value,
                LocalityId = eventToParse.Locality.Id.Value,
                Private = eventToParse.Private,
                UserId = eventToParse.User.Id.Value,
                EventTypeId = eventToParse.EventType.Id.Value,
            };
        }

        #endregion
    }
}
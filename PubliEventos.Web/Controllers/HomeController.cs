namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Event;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de eventos.
        /// </summary>
        [Dependency]
        public IEventServices serviceEvents { get; set; }

        #endregion

        #region Views

        /// <summary>
        /// Vista index.
        /// </summary>
        /// <returns>Index view.</returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.events = serviceEvents.SearchFilteredEvents(new SearchFilteredEventsRequest() { StartDate = DateTime.Now.AddMonths(-3), EndDate = DateTime.Now.AddMonths(6), SearchPublics = true }).Events
                .OrderByDescending(x => x.EventDate)
                .ToList();

            ViewBag.eventTypes = new SelectList(serviceEvents.GetAllEventTypes(), "Id", "Description");

            return View();
        }

        /// <summary>
        /// Vista de búsqueda de eventos por aproximación.
        /// </summary>
        /// <returns>SearchEventsByDistance view.</returns>
        [AllowAnonymous]
        public ActionResult SearchEventsByDistance()
        {
            return View();
        }

        #endregion

        #region

        /// <summary>
        /// Búsqueda de eventos mediantes filtros.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="eventType">Tipo de evento.</param>
        /// <param name="startDate">Fecha desde.</param>
        /// <param name="endDate">Fecha hasta.</param>
        /// <param name="fullText">Búsqueda por texto.</param>
        /// <returns>Eventos.</returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SearchEvents(int? userId, int? eventType, DateTime? startDate, DateTime? endDate, string fullText, bool? initialSearch)
        {
            var events = serviceEvents.SearchFilteredEvents(new SearchFilteredEventsRequest()
            {
                StartDate = initialSearch.HasValue && initialSearch.Value ? DateTime.Now.AddMonths(-3) : startDate,
                EndDate = initialSearch.HasValue && initialSearch.Value ? DateTime.Now.AddMonths(6) : endDate,
                SearchPublics = true,
                EventTypeId = eventType,
                UserId = userId,
                SearchTerm = fullText
            }).Events
              .OrderByDescending(x => x.EventDate)
              .ToList();

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(events);

            return Json(new { Events = json }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Busca eventos por distancia.
        /// </summary>
        /// <param name="latitudeInitial">Latitud inicial.</param>
        /// <param name="longitudeInitial">Longitud inicial.</param>
        /// <param name="maxDistance">Distancia máxima.</param>
        /// <returns>Eventos encontrados.</returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SearchEventsByDistance(string latitudeInitial, string longitudeInitial, int maxDistance)
        {
            var events = this.serviceEvents.SearchEventsByDistance(new SearchEventsByDistanceRequest()
            {
                LatitudeInitial = latitudeInitial,
                LongitudeInitial = longitudeInitial,
                MaxDistance = maxDistance
            }).Events;

            return Json(new { Events = events }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}

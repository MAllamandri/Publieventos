namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Event;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    public class HomeController : Controller
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

        #endregion

    }
}

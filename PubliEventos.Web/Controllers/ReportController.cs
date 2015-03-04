namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Services.Report;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de reportes.
    /// </summary>
    [Authorize]
    public class ReportController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de eventos.
        /// </summary>
        [Dependency]
        public IReportService serviceReports { get; set; }

        #endregion

        [HttpPost]
        public JsonResult ReportContent(ReportContentRequest model)
        {
            if (ModelState.IsValid)
            {
                //Seteo el usuario logueado, el que realizo el reporte.
                model.UserId = User.Id;

                this.serviceReports.ReportContent(model);

                // TODO > Ver cantidad de reportes para desactiar evento.

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}

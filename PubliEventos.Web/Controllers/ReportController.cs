﻿namespace PubliEventos.Web.Controllers
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

        #region Json Methods

        /// <summary>
        /// Reportar un contenido.
        /// </summary>
        /// <param name="model">ReportContentRequest model.</param>
        /// <returns>true o false.</returns>
        [HttpPost]
        public JsonResult ReportContent(ReportContentRequest model)
        {
            if (ModelState.IsValid)
            {
                //Seteo el usuario logueado, el que realizo el reporte.
                model.UserId = User.Id;

                //Doy de alta el reporte.
                this.serviceReports.ReportContent(model);

                //Evaluo el elemento reportado para ver si lo debo desactivar.
                var isDisabled = this.serviceReports.EvaluateReportsForDisabled(new EvaluateReportsForDisabledRequest()
                {
                    ContentId = model.ContentId,
                    ContentType = model.ContentType,
                    EventId = model.EventId
                }).IsDisabled;

                return Json(new { Success = true, IsDisabled = isDisabled }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Administración de los contenidos reportados.
        /// </summary>
        /// <param name="model">AdministrationReportedRequest model.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AdministrationReported(AdministrationReportedRequest model)
        {
            if (ModelState.IsValid)
            {
                this.serviceReports.AdministrationReported(model);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

namespace PubliEventos.Web.Controllers
{
    using Microsoft.Practices.Unity;
    using PubliEventos.Contract.Contracts;
    using PubliEventos.Contract.Enums;
    using PubliEventos.Contract.Services.Report;
    using PubliEventos.Web.Mvc.Filters;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador del administrador.
    /// </summary>
    [Authorize]
    public class AdminController : BaseController
    {
        #region Properties

        /// <summary>
        /// Servicio de eventos.
        /// </summary>
        [Dependency]
        public IReportService serviceReports { get; set; }

        #endregion

        /// <summary>
        /// Vista de administración de contenidos reportados.
        /// </summary>
        /// <returns>ReportContents view.</returns>
        [UserActionRestriction(ValidateCondition.IsAdministrator)]
        public ActionResult ReportContents()
        {
            var model = this.serviceReports.SearchReportedContents(new SearchReportedContentsRequest() { CurrentUserId = User.Id });

            return View(model);
        }

        /// <summary>
        /// Obtiene las razones de los reportes de un contenido.
        /// </summary>
        /// <param name="model">SearchReasonsByContentReportedRequest model.</param>
        /// <returns>Reportes.</returns>
        public JsonResult SearchReasonsByContentReported(SearchReasonsByContentReportedRequest model)
        {
            var reports = this.serviceReports.SearchReasonsByContentReported(model).Reports;

            return Json(new { Reports = reports }, JsonRequestBehavior.AllowGet);
        }
    }
}

namespace PubliEventos.Web.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de errores.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Vista de acceso no permitido.
        /// </summary>
        /// <returns>UnauthorizedAccess view.</returns>
        public ActionResult UnauthorizedAccess()
        {
            return View();
        }
    }
}

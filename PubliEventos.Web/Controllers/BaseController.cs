namespace PubliEventos.Web.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using PubliEventos.Web.App_Start;

    public class BaseController : Controller
    {
        /// <summary>
        /// Obtiene el usuario logueado.
        /// </summary>
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}

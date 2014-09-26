namespace PubliEventos.Web.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using PubliEventos.Web.App_Start;

    public class BaseController : Controller
    {
        /// <summary>
        /// Path de almacenamiento de las fotos de portada.
        /// </summary>
        public string pathCoverPhoto
        {
            get { return "/Content/Images/Covers/"; }
        }

        /// <summary>
        /// Obtiene el usuario logueado.
        /// </summary>
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}

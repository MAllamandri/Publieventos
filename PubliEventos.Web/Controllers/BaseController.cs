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
        /// Path de almacenamiento de las foto de perfil.
        /// </summary>
        public string pathImageProfile
        {
            get { return "/Content/images/Profiles/"; }
        }

        /// <summary>
        /// Paht de almacenamiento de fotos y videos de los eventos.
        /// </summary>
        public string pathEventsPictures
        {
            get { return "/Content/images/EventsPictures/"; }
        }

        /// <summary>
        /// Obtiene el usuario logueado.
        /// </summary>
        protected virtual new CustomPrincipal User
        {
            get { return System.Web.HttpContext.Current.User as CustomPrincipal; }
        }
    }
}

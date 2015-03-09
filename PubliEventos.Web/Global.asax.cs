using log4net.Config;
using PubliEventos.DataAccess.Infrastructure;
using PubliEventos.Web.App_Start;
using PubliEventos.Web.Hubs;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace PubliEventos.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BaseHubs.Initialise();
            RouteTable.Routes.MapHubs();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            XmlConfigurator.Configure();
            Bootstrapper.Initialise();
        }

        protected SessionHelper _sessionHelper = null;

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            _sessionHelper = new SessionHelper();
            _sessionHelper.OpenSession();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            _sessionHelper = new SessionHelper();
            _sessionHelper.CloseSession();
        }

        /// <summary>
        /// Obtengo los datos del usuario.
        /// </summary>
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                var serializer = new JavaScriptSerializer();

                var serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);

                var newUser = new CustomPrincipal(authTicket.Name)
                {
                    Id = serializeModel.Id,
                    FirstName = serializeModel.FirstName,
                    LastName = serializeModel.LastName,
                    ImageProfile = string.IsNullOrEmpty(serializeModel.ImageProfile) ? "contact-default-image.jpg" : serializeModel.ImageProfile
                };

                HttpContext.Current.User = newUser;
            }
        }
    }
}
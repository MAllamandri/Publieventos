using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net.Config;
using PubliEventos.DataAccess.Infrastructure;

namespace PubliEventos.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
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
    }
}
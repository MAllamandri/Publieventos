using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PubliEventos.Web.Controllers
{
    public class EventController : BaseController
    {
        public ActionResult Create()
        {
            return View();
        }
    }
}

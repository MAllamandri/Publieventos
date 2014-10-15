using PubliEventos.Contract.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PubliEventos.Web.Models.EventModels
{
    public class FilterModel
    {
        public SelectList Provinces { get; set; }

        public SelectList Localities { get; set; }

        public SelectList EventTypes { get; set; }
    }
}
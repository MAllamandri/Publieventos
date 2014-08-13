using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PubliEventos.Domain.Domain;

namespace PubliEventos.Web.Models
{
    public class EventsModel
    {
        public List<Event> events { get; set; }
    }
}
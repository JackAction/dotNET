using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spielzeuge.Models
{
    public class Reservierung
    {
        public int ReservierungId { get; set; }

        public int SpielzeugId { get; set; }

        public Spielzeug Spielzeug { get; set; }

    }
}
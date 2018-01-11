﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Spielzeuge.Models
{
    public class Spielzeug
    {
        public int SpielzeugId { get; set; }
        [DisplayName("Spielzeug")]
        public string Name { get; set; }
        public double Preis { get; set; }
        public string Details { get; set; }
        public bool Aktiv { get; set; }
        public bool Ausgeliehen { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Spielzeugverleih.Models
{
    public class Spielzeug
    {
        public int SpielzeugId { get; set; }
        [DisplayName("Spielzeug")]
        public string Name { get; set; }
        public double Preis { get; set; }
    }
}
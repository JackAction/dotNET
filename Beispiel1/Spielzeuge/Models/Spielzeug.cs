using System;
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
        public bool Aktiv { get; set; } // Anstelle von löschen -> aktiv/inaktiv (nur Admin)
        public bool Ausgeliehen { get; set; } // 1 = Spielzeug abgeholt, 0 = Spielzueg noch nicht abgeholt oder zurückgebracht (nur Admin)

        public List<Reservierung> Reservierungen { get; set; }

        public List<Bild> Bilder { get; set; }
    }
}
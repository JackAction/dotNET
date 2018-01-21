using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spielzeuge.Models
{
    public class Bild
    {
        public int BildId { get; set; }

        public int SpielzeugId { get; set; }

        public byte[] ImageByte { get; set; }

        public Spielzeug Spielzeug { get; set; }
    }
}
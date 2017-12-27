using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Music.Models
{
    public class Sprache
    {
        public virtual string SpracheId { get; set; }
        public virtual string Name { get; set; }
        public virtual List<Album> Albums { get; set; }

    }
}
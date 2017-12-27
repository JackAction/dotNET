using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aufagbe2.Models
{
    public class Velo
    {
        public virtual int VeloId { get; set; }
        public virtual string Description { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
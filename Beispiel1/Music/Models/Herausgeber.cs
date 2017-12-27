using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Music.Models
{
    public class Herausgeber
    {
        public virtual int HerausgeberId { get; set; }
        [DisplayName("HerausgeberAnotation")]
        public virtual string Name { get; set; }
    }
}
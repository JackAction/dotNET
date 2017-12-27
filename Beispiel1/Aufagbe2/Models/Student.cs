using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Aufagbe2.Models
{
    public class Student
    {

        //public Student()
        //{
        //    Velos = new List<Velo>();
        //}

        public virtual int StudentId { get; set; }
        public virtual string Name { get; set; }

        public virtual ICollection<Velo> Velos { get; set; }

        [NotMapped]
        public int[] SelectedVeloIDs { get; set; }
    }
}
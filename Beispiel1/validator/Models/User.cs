using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace validator.Models
{
    public class User : IValidatableObject
    {
        [Key]
        [Required]
        public virtual string LoginId { get; set; }
        [Required]
        public virtual string Vorname { get; set; }
        [Required]
        public virtual string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(LoginId.Contains(Vorname) ^ LoginId.Contains(Name)))
            {
                yield return new ValidationResult("no, no no no"); 
            }
        }
    }
}
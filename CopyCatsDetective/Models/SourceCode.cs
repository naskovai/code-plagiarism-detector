using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    public class SourceCode
    {
        public string Id { get; set; }
        public int CoderId { get; set; }
        [Required]
        public string ActualCode { get; set; }
        [Required]
        public string Language { get; set; }

        public virtual Coder Coder { get; set; }
    }
}
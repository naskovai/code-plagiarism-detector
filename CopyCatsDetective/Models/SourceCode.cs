using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    public class SourceCode
    {
        public string Id { get; set; }
        public int CoderId { get; set; }
        public string ActualCode { get; set; }
        public string Language { get; set; }

        public virtual Coder Coder { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    public class Coder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public int NumberOfSourceCodes { get; set; }
        public float CheaterIndex { get; set; }
    }
}
using CopyCatsDetective.Controllers.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    public class CompareViewModel
    {
        [Required]
        [Display(Name = "Choose Language")]
        public Languages Language { get; set; }

        [Required]
        [Display(Name = "Source Code 1")]
        [DataType(DataType.MultilineText)]
        public string FirstSourceCode { get; set; }

        [Required]
        [Display(Name = "Source Code 2")]
        [DataType(DataType.MultilineText)]
        public string SecondSourceCode { get; set; }
    }

    public class ComparisonResultViewModel
    {
        public double Similarity { get; set; }

        public bool AreEqual { get; set; }

        public string ResultMessage { get; set; }
    }
}
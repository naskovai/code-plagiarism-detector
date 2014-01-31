using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    [Table("CodePool")]
	public class CodePool
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Category Category { get; set; }
	}

    public class CreateCodePoolViewModel
    {
        [Required]
        public string Language { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
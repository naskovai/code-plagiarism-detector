using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public virtual Category ParentCategory { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<CodePool> CodePools { get; set; }
    }

    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int ParentCategoryId { get; set; }
    }
}
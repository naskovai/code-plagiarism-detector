using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int ParentCategoryId { get; set; }

        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<CodePool> CodePools { get; set; }
    }
}
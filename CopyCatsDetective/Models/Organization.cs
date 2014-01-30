using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    [Table("Organization")]
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    [Table("Organization_Member")]
    public class Organization_Member
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [Required]
        public virtual ApplicationUser Member { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
    }
}
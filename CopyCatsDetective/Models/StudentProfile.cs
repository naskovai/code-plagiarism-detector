using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    [Table("StudentProfile")]
    public class StudentProfile
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime CheaterPointsUpdated { get; set; }

        public int CheaterPoints { get; set; }

        public int CheaterLevel { get; set; }

        public virtual Organization Organization { get; set; }
    }

    public class StudentViewModel
    {
        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        public Organization Organization { get; set; }

        public int CheaterLevel { get; set; }

        public int CheaterPoints { get; set; }
    }
}
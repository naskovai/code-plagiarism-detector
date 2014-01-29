using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
    }
}
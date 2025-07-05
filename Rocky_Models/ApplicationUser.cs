using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Identity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;


namespace Rocky_Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [NotMapped]
        public string StreetAddress { get; set; }
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string State { get; set; }
        [NotMapped]
        public string PostalCode { get; set; }
    }
}

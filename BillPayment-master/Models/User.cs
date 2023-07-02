using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BillPayment.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Firstname is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is required")]
        public string LastName { get; set; }
        public string ContactNumber { get; set; }  
        [Required(ErrorMessage = "Username is compulsory")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is compulsory")]
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string Maplocation { get; set; }
        public string Role { get; set; }
        public bool Validate { get; set; }

        public string VCode { get; set; }
    }

}
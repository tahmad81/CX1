using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class AppUserVM
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        [Display(Name = "Full Name")]
        public string Full_Name { get; set; }
        public string Email { get; set; }
        [Display(Name = "Signup Date")]
        public DateTime? Signup_Date { get; set; }
    }
}
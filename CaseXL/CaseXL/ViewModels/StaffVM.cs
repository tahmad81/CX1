using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class StaffVM : Infrastructure.FirmBase
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Frist Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Partner Name")]
        public string PartnerName { get; set; }
        //  [ScaffoldColumn(false)]  
        [Required(ErrorMessage = "Please Select Partner")]
        public int PartnerID { get; set; }

    }
}
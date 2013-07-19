using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class WitnessVM
    {
        public int WitnessId { get; set; }
     
        public int ID { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string FullName { get; set; }
        //[RegularExpression(@"^(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})$",ErrorMessage ="Not valid Email")]
        public string Email { get; set; }
        public bool IsSelected { get; set; }
        public int? Issue_Witness_Id { get; set; }
        [Display(Name = "Docs Authored")]
        public string Docs_Authored { get; set; }
        [Display(Name = "Docs Received")]
        public string Docs_Received { get; set; }
        
    }
}
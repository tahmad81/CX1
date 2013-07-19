using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class ExhibitVM
    {
        public DateTime ? Date { get; set; }
       [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Author { get; set; }
        public string Recipient { get; set; }
        public string Source { get; set; }
     
        public int Exhibit_Id { get; set; }
        public bool IsSelected { get; set; }
        public int ? Fact_Exhibit { get; set; }
        [Required(ErrorMessage = "Type is required")]
       [Display(Name="Type")]
        public int Type_Id { get; set; }
        public int ? CaseId { get; set; }
     
       
    }
}
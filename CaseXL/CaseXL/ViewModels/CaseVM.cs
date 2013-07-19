using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class CaseVM : Infrastructure.ComboModelBase
    {
        [Required(ErrorMessage = "Case Caption is required")]
        public string Caption { get; set; }

        public int Case_Id { get; set; }

        [Display(Name = "Case Number"), Required(ErrorMessage = "Case Number is required")]
        public string Case_Number { get; set; }

        [Required(ErrorMessage = "Case Type is required"), Display(Name = "Case Type")]
        public int Case_Type { get; set; }


    }
}
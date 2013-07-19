using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaseXL.ViewModels
{
    public class FactWitnessQuestionVM : QuestionVM
    {
        // Methods
        public FactWitnessQuestionVM()
        { 
        
        }

        // Properties
        [Display(Name = "Asked ?")]
        public bool Asked { get; set; }
        [Display(Name = "Question#")]
        public double Question_No  { get; set; }
    }


}

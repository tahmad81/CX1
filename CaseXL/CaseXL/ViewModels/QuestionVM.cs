using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class QuestionVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Question is required")]
        public string Question { get; set; }
        [Required(ErrorMessage = "Answer is required")]
        public string Answer { get; set; }
        public int ? Witness_Id { get; set; }
        public bool IsAsked { get; set; }
        public int ? Issue_Id { get; set; }
        public int Fact_Id { get; set; }

    }
}
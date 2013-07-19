using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class IssuesVM
    {
        public int Case_ID { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Issue name is erquired")]
        [Display(Name = "Issue Name")]
        public string Issue_Name { get; set; }
        public int Case_Type { get; set; }
        [Display(Name = "Case Type")]
        public string Case_Type_Name { get; set; }
        //[Required(ErrorMessage = "Issue Explanation is required")]
        [Display(Name = "Issue Explanation")]
        public string Issue_Exp { get; set; }
        [Display(Name = "Case Name")]
        public string Case_Name { get; set; }
        [Display(Name = "Case Name")]
        public string SelectedCase { get; set; }
        public List<CaseXL.ViewModels.IssuesVM> Issues { get; set; }
        [Display(Name = "Question")]
        public string Issue_Question { get; set; }
        [Display(Name = "This Issue is")]
        public string Issue_Evaluation { get; set; }
        public bool IsSelected { get; set; }
        public int? FactID { get; set; }
        public int? Issue_Fact_Id { get; set; }
        public string Description { get; set; }
        public IssuesVM()
        {
            this.Issues = new List<IssuesVM>();
        }
    }
}
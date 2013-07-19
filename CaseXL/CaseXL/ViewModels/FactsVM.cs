using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class FactsVM
    {
        public int ID { get; set; }
        public int CaseID { get; set; }
        [Required(ErrorMessage = "Date is required")]

        private DateTime? _date;
        public DateTime? Date
        {
            get { return this._date; }
            set
            {
                if (value != null)
                {
                    this._date = new DateTime(value.Value.Ticks, DateTimeKind.Utc);
                }
                else
                    this._date = value;
            }
        }

        public DateTime? DateValue { get; set; }
        [Display(Name = "Fact Text")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string Source { get; set; }
        public string Evaluation { get; set; }
        [Display(Name = "Linked Issues")]
        public string Linked_Issues { get; set; }
        [Required(ErrorMessage = "Please Select Evaluation")]
        public int Evaluation_Id { get; set; }
        [Display(Name = "Evaluation")]
        public string Evaluation_Text { get; set; }
    }
}
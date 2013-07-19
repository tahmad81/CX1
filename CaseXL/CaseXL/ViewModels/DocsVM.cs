using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseXL.ViewModels
{
    public class DocsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int Parent_Id { get; set; }
    }

}
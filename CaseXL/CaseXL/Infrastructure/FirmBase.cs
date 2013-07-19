using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseXL.Infrastructure
{
    public class FirmBase
    {
        int _id;
        public int Firm_ID
        {
            get { return this._id; }
            set { this._id = value; }
        }
        string _name;
        public string Firm_Name
        {   
            get { return this._name; }
            set { this._name = value; }
        }
    }
}
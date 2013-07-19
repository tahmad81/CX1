using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CaseXL.Infrastructure
{
    public class ComboModelBase
    {
        string _name;
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        int? _id;
        public int? ID
        {
            get { return this._id; }
            set { this._id = value; }
        }
    }
}
#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using CaseXL.Data;


namespace CaseXL.Data	
{
	public partial class Firm
	{
		private int _iD;
		public virtual int ID 
		{ 
		    get
		    {
		        return this._iD;
		    }
		    set
		    {
		        this._iD = value;
		    }
		}
		
		private string _firm_Name;
		public virtual string Firm_Name 
		{ 
		    get
		    {
		        return this._firm_Name;
		    }
		    set
		    {
		        this._firm_Name = value;
		    }
		}
		
		private IList<Staff> _staffs = new List<Staff>();
		public virtual IList<Staff> Staffs 
		{ 
		    get
		    {
		        return this._staffs;
		    }
		}
		
		private IList<Partner> _partners = new List<Partner>();
		public virtual IList<Partner> Partners 
		{ 
		    get
		    {
		        return this._partners;
		    }
		}
		
	}
}

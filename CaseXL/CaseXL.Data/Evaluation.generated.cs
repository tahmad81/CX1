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
	public partial class Evaluation
	{
		private int _id;
		public virtual int Id 
		{ 
		    get
		    {
		        return this._id;
		    }
		    set
		    {
		        this._id = value;
		    }
		}
		
		private string _evaluation_Text;
		public virtual string Evaluation_Text 
		{ 
		    get
		    {
		        return this._evaluation_Text;
		    }
		    set
		    {
		        this._evaluation_Text = value;
		    }
		}
		
		private IList<Fact> _facts = new List<Fact>();
		public virtual IList<Fact> Facts 
		{ 
		    get
		    {
		        return this._facts;
		    }
		}
		
	}
}

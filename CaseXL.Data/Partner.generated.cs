#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ClassGenerator.ttinclude code generation file.
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
	public partial class Partner
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
		
		private string _fristName;
		public virtual string FristName
		{
			get
			{
				return this._fristName;
			}
			set
			{
				this._fristName = value;
			}
		}
		
		private string _lastName;
		public virtual string LastName
		{
			get
			{
				return this._lastName;
			}
			set
			{
				this._lastName = value;
			}
		}
		
		private int _firm_ID;
		public virtual int Firm_ID
		{
			get
			{
				return this._firm_ID;
			}
			set
			{
				this._firm_ID = value;
			}
		}
		
		private Firm _firm;
		public virtual Firm Firm
		{
			get
			{
				return this._firm;
			}
			set
			{
				this._firm = value;
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
		
	}
}
#pragma warning restore 1591

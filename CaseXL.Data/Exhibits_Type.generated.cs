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
	public partial class Exhibits_Type
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
		
		private string _typer;
		public virtual string Typer
		{
			get
			{
				return this._typer;
			}
			set
			{
				this._typer = value;
			}
		}
		
		private IList<Exhibit> _exhibits = new List<Exhibit>();
		public virtual IList<Exhibit> Exhibits
		{
			get
			{
				return this._exhibits;
			}
		}
		
	}
}
#pragma warning restore 1591

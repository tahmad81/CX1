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
	public partial class Issue
	{
		private int _case_ID;
		public virtual int Case_ID 
		{ 
		    get
		    {
		        return this._case_ID;
		    }
		    set
		    {
		        this._case_ID = value;
		    }
		}
		
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
		
		private string _issue_Expl;
		public virtual string Issue_Expl 
		{ 
		    get
		    {
		        return this._issue_Expl;
		    }
		    set
		    {
		        this._issue_Expl = value;
		    }
		}
		
		private string _issue_Name;
		public virtual string Issue_Name 
		{ 
		    get
		    {
		        return this._issue_Name;
		    }
		    set
		    {
		        this._issue_Name = value;
		    }
		}
		
		private long _dateCreated;
		public virtual long DateCreated 
		{ 
		    get
		    {
		        return this._dateCreated;
		    }
		    set
		    {
		        this._dateCreated = value;
		    }
		}
		
		private string _description;
		public virtual string Description 
		{ 
		    get
		    {
		        return this._description;
		    }
		    set
		    {
		        this._description = value;
		    }
		}
		
		private string _evaluation;
		public virtual string Evaluation 
		{ 
		    get
		    {
		        return this._evaluation;
		    }
		    set
		    {
		        this._evaluation = value;
		    }
		}
		
		private string _question;
		public virtual string Question 
		{ 
		    get
		    {
		        return this._question;
		    }
		    set
		    {
		        this._question = value;
		    }
		}
		
		private int _issue_Witness;
		public virtual int Issue_Witness 
		{ 
		    get
		    {
		        return this._issue_Witness;
		    }
		    set
		    {
		        this._issue_Witness = value;
		    }
		}
		
		private int _fact_Id;
		public virtual int Fact_Id 
		{ 
		    get
		    {
		        return this._fact_Id;
		    }
		    set
		    {
		        this._fact_Id = value;
		    }
		}
		
		private IList<Issues_Fact> _issues_Facts = new List<Issues_Fact>();
		public virtual IList<Issues_Fact> Issues_Facts 
		{ 
		    get
		    {
		        return this._issues_Facts;
		    }
		}
		
		private IList<Issue_Question> _issue_Questions = new List<Issue_Question>();
		public virtual IList<Issue_Question> Issue_Questions 
		{ 
		    get
		    {
		        return this._issue_Questions;
		    }
		}
		
	}
}

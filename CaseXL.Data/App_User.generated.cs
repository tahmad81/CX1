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
	public partial class App_User
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
		
		private string _firstName;
		public virtual string FirstName
		{
			get
			{
				return this._firstName;
			}
			set
			{
				this._firstName = value;
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
		
		private string _creditCard;
		public virtual string CreditCard
		{
			get
			{
				return this._creditCard;
			}
			set
			{
				this._creditCard = value;
			}
		}
		
		private long _subscriptionNo;
		public virtual long SubscriptionNo
		{
			get
			{
				return this._subscriptionNo;
			}
			set
			{
				this._subscriptionNo = value;
			}
		}
		
		private string _userName;
		public virtual string UserName
		{
			get
			{
				return this._userName;
			}
			set
			{
				this._userName = value;
			}
		}
		
		private int _firmId;
		public virtual int FirmId
		{
			get
			{
				return this._firmId;
			}
			set
			{
				this._firmId = value;
			}
		}
		
		private string _cVNNo;
		public virtual string CVNNo
		{
			get
			{
				return this._cVNNo;
			}
			set
			{
				this._cVNNo = value;
			}
		}
		
		private string _email;
		public virtual string Email
		{
			get
			{
				return this._email;
			}
			set
			{
				this._email = value;
			}
		}
		
		private string _contactNo;
		public virtual string ContactNo
		{
			get
			{
				return this._contactNo;
			}
			set
			{
				this._contactNo = value;
			}
		}
		
		private DateTime? _signupdate;
		public virtual DateTime? Signupdate
		{
			get
			{
				return this._signupdate;
			}
			set
			{
				this._signupdate = value;
			}
		}
		
		private bool _is_Trial;
		public virtual bool Is_Trial
		{
			get
			{
				return this._is_Trial;
			}
			set
			{
				this._is_Trial = value;
			}
		}
		
		private bool _is_Client;
		public virtual bool Is_Client
		{
			get
			{
				return this._is_Client;
			}
			set
			{
				this._is_Client = value;
			}
		}
		
		private DateTime? _exp_Date;
		public virtual DateTime? Exp_Date
		{
			get
			{
				return this._exp_Date;
			}
			set
			{
				this._exp_Date = value;
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
		
		private IList<Client_Case> _client_Cases = new List<Client_Case>();
		public virtual IList<Client_Case> Client_Cases
		{
			get
			{
				return this._client_Cases;
			}
		}
		
	}
}
#pragma warning restore 1591

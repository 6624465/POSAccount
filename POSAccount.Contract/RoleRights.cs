using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using POSAccount.Contract;

namespace POSAccount.Contract
{
	public class RoleRights: IContract
	{
		// Constructor 
		public RoleRights() { }

		// Public Members 

		[DisplayName("RoleCode")] 
		public string  RoleCode { get; set; }

		[DisplayName("SecurableItem")] 
		public string  SecurableItem { get; set; }


	}
}




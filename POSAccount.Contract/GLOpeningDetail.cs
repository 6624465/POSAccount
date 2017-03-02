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
	public class GLOpeningDetail: IContract
	{
		// Constructor 
		public GLOpeningDetail() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("AccountCode")] 
		public string  AccountCode { get; set; }

        [DisplayName("AccountCodeDescription")]
        public string AccountCodeDescription { get; set; }


        

		[DisplayName("Remark")] 
		public string Remark { get; set; }

		[DisplayName("DebitAmount")] 
		public decimal DebitAmount { get; set; }

		[DisplayName("CreditAmount")] 
		public decimal CreditAmount { get; set; }


	}
}




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
	public class GLJournalDetail: IContract
	{
		// Constructor 
		public GLJournalDetail() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("ItemNo")] 
		public Int16  ItemNo { get; set; }

		[DisplayName("AccountCode")] 
		public string  AccountCode { get; set; }

        [DisplayName("AccountCodeDescription")]
        public string AccountCodeDescription { get; set; }
        
		[DisplayName("Remark")] 
		public string Remark { get; set; }

		[DisplayName("BaseDebitAmount")] 
		public decimal BaseDebitAmount { get; set; }

		[DisplayName("BaseCreditAmount")] 
		public decimal BaseCreditAmount { get; set; }


        [DisplayName("Status")]
        public bool Status { get; set; }


	}
}




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
	public class GLOpening: IContract
	{
		// Constructor 
		public GLOpening() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("FinancialYear")] 
		public Int32  FinancialYear { get; set; }

		[DisplayName("BranchID")] 
		public Int16  BranchID { get; set; }

		[DisplayName("AccountDate")] 
		public DateTime  AccountDate { get; set; }

		[DisplayName("TotalDebitAmount")] 
		public decimal TotalDebitAmount { get; set; }

		[DisplayName("TotalCreditAmount")] 
		public decimal TotalCreditAmount { get; set; }

		[DisplayName("CreatedBy")] 
		public string  CreatedBy { get; set; }

		[DisplayName("CreatedOn")] 
		public DateTime  CreatedOn { get; set; }

		[DisplayName("ModifiedBy")] 
		public string  ModifiedBy { get; set; }

		[DisplayName("ModifiedOn")] 
		public DateTime  ModifiedOn { get; set; }

        public List<GLOpeningDetail> GLOpenDetails { get; set; }
        public List<GLTransaction> GLTransactionDetails { get; set; }


        [DisplayName("AccountCode")]
        public string AccountCode { get; set; }


        [DisplayName("Debit/Credit")]
        public string DebitCredit { get; set; }


        [DisplayName("Amount")]
        public string Amount { get; set; }


        [DisplayName("DetailRemark")]
        public string DetailRemark { get; set; }


        public IEnumerable<SelectListItem> AccountCodeList { get; set; }
        public IEnumerable<SelectListItem> DebitCreditList { get; set; }
	}
}




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
	public class GLJournal: IContract
	{
		// Constructor 
		public GLJournal() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("DocumentDate")] 
		public DateTime  DocumentDate { get; set; }


        [DisplayName("BranchID")]
        public Int16 BranchID { get; set; }

        

		[DisplayName("Remark")] 
		public string Remark { get; set; }

		[DisplayName("CurrencyCode")] 
		public string  CurrencyCode { get; set; }

		[DisplayName("ExchangeRate")] 
		public decimal ExchangeRate { get; set; }

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

		[DisplayName("Source")] 
		public string  Source { get; set; }

		[DisplayName("IsCancel")] 
		public bool  IsCancel { get; set; }

		[DisplayName("CancelBy")] 
		public string  CancelBy { get; set; }

		[DisplayName("CancelOn")] 
		public DateTime  CancelOn { get; set; }

		[DisplayName("CancelReason")] 
		public string CancelReason { get; set; }

        public List<GLJournalDetail> GLJournalDetails { get; set; }
        public List<GLTransaction> GLTransactionDetails { get; set; }


        [DisplayName("AccountCode")]
        public string AccountCode { get; set; }


        [DisplayName("Debit/Credit")]
        public string DebitCredit { get; set; }


        [DisplayName("Amount")]
        public decimal Amount { get; set; }


        [DisplayName("DetailRemark")]
        public string DetailRemark  { get; set; }


        public IEnumerable<SelectListItem> AccountCodeList { get; set; }
        public IEnumerable<SelectListItem> DebitCreditList { get; set; }

	}
}




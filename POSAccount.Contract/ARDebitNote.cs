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
	public class ARDebitNote: IContract
	{
		// Constructor 
		public ARDebitNote() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("DocumentDate")] 
		public DateTime  DocumentDate { get; set; }

		[DisplayName("BranchID")] 
		public Int16  BranchID { get; set; }

		[DisplayName("ReferenceNo")] 
		public string  ReferenceNo { get; set; }

		[DisplayName("DebtorCode")] 
		public string  DebtorCode { get; set; }

        [DisplayName("AccountCode")]
        public string AccountCode { get; set; }

        [DisplayName("DebtorName")]
        public string DebtorName { get; set; }

		[DisplayName("CreditTerm")] 
		public string  CreditTerm { get; set; }

		[DisplayName("CurrencyCode")] 
		public string  CurrencyCode { get; set; }

		[DisplayName("ExchangeRate")] 
		public decimal ExchangeRate { get; set; }

		[DisplayName("DebtorAccount")] 
		public string  DebtorAccount { get; set; }

		[DisplayName("BaseAmount")] 
		public decimal BaseAmount { get; set; }

		[DisplayName("LocalAmount")] 
		public decimal LocalAmount { get; set; }

		[DisplayName("TaxAmount")] 
		public decimal TaxAmount { get; set; }

		[DisplayName("TotalAmount")] 
		public decimal TotalAmount { get; set; }

		[DisplayName("Remark")] 
		public string  Remark { get; set; }

		[DisplayName("Source")] 
		public string  Source { get; set; }

		[DisplayName("IsCancel")] 
		public bool  IsCancel { get; set; }

		[DisplayName("CreatedBy")] 
		public string  CreatedBy { get; set; }

		[DisplayName("CreatedOn")] 
		public DateTime  CreatedOn { get; set; }

		[DisplayName("ModifiedBy")] 
		public string  ModifiedBy { get; set; }

		[DisplayName("ModifiedOn")] 
		public DateTime  ModifiedOn { get; set; }

		[DisplayName("CancelledBy")] 
		public string  CancelledBy { get; set; }

		[DisplayName("CancelledOn")] 
		public DateTime  CancelledOn { get; set; }

		[DisplayName("CancelReason")] 
		public string CancelReason { get; set; }


        public List<ARDebitNoteDetail> ARDebitNoteDetails { get; set; }

        public IEnumerable<SelectListItem> DebtorList { get; set; }

        public IEnumerable<SelectListItem> CurrencyCodeList { get; set; }

        public IEnumerable<SelectListItem> DebtorAccountList { get; set; }

        public List<GLTransaction> GLTransactionDetails { get; set; }

        public IEnumerable<SelectListItem> AccountCodeList { get; set; }

	}
}




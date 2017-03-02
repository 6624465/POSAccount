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
	public class CBBankTransfer: IContract
	{
		// Constructor 
		public CBBankTransfer() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("BranchID")] 
		public Int16  BranchID { get; set; }


        [DisplayName("DocumentDate")]
        public DateTime DocumentDate { get; set; }
    
        [DisplayName("ReferenceNo")] 
		public string ReferenceNo { get; set; }

		[DisplayName("ChequeNo")] 
		public string ChequeNo { get; set; }

		[DisplayName("ChequeDate")] 
		public DateTime  ChequeDate { get; set; }

		[DisplayName("FromBankCode")] 
		public string  FromBankCode { get; set; }

		[DisplayName("FromBankAccount")] 
		public string  FromBankAccount { get; set; }

		[DisplayName("ToBankCode")] 
		public string  ToBankCode { get; set; }

		[DisplayName("ToBankAccount")] 
		public string  ToBankAccount { get; set; }

		[DisplayName("BankChargesAccount")] 
		public string  BankChargesAccount { get; set; }

		[DisplayName("WithDrawAmount")] 
		public decimal WithDrawAmount { get; set; }

		[DisplayName("DepositAmount")] 
		public decimal DepositAmount { get; set; }

		[DisplayName("Remarks")] 
		public string Remarks { get; set; }

		[DisplayName("Source")] 
		public string  Source { get; set; }

		[DisplayName("CreatedBy")] 
		public string  CreatedBy { get; set; }

		[DisplayName("CreatedOn")] 
		public DateTime  CreatedOn { get; set; }

		[DisplayName("ModifiedBy")] 
		public string  ModifiedBy { get; set; }

		[DisplayName("ModifiedOn")] 
		public DateTime  ModifiedOn { get; set; }

		[DisplayName("IsCancel")] 
		public bool  IsCancel { get; set; }

		[DisplayName("CancelBy")] 
		public string  CancelBy { get; set; }

		[DisplayName("CancelOn")] 
		public DateTime  CancelOn { get; set; }



        public IEnumerable<SelectListItem> BankCodeList { get; set; }
    }
}




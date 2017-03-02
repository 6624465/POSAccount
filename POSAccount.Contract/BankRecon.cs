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
	public class BankRecon: IContract
	{
		// Constructor 
		public BankRecon() { }

        [DisplayName("Account Date")] 
        public DateTime AccountDate { get; set; }

        [DisplayName("Source")] 
        public string Source { get; set; }

        [DisplayName("Document Date")] 
        public DateTime DocumentDate { get; set; }

        [DisplayName("Document Type")] 
        public string DocumentType { get; set; }

        [DisplayName("Document No")] 
        public string DocumentNo { get; set; }

        [DisplayName("Cheque No")] 
        public string ChequeNo { get; set; }

        [DisplayName("BankInSlip No")] 
        public string BankInSlipNo { get; set; }

        [DisplayName("Date Reconciled")] 
        public DateTime DateReconciled { get; set; }

        [DisplayName("BankStatement PgNo")] 
        public string BankStatementPgNo { get; set; }

        [DisplayName("Local Amount")] 
        public decimal LocalAmount { get; set; }


        [DisplayName("Receipt Amount")]
        public decimal ReceiptAmount{ get; set; }

        [DisplayName("Payment Amount")]
        public decimal PaymentAmount { get; set; }


        [DisplayName("Remark")] 
        public string Remark { get; set; }

        [DisplayName("BranchID")]
        public short BranchID { get; set; }

        [DisplayName("BankCode")]
        public string BankCode { get; set; }

        [DisplayName("StartDate")]
        public DateTime StartDate { get; set; }

        [DisplayName("EndDate")]
        public DateTime EndDate { get; set; }

        public IEnumerable<SelectListItem> BankCodeList { get; set; }

    }
}

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
	public class GLTransaction: IContract
	{
		// Constructor 
		public GLTransaction() { }

		// Public Members 

		[DisplayName("TransactionNo")] 
		public string  TransactionNo { get; set; }

		[DisplayName("ItemNo")] 
		public Int16  ItemNo { get; set; }


        [DisplayName("BranchID")]
        public Int16 BranchID { get; set; }

		[DisplayName("AccountCode")] 
		public string  AccountCode { get; set; }

		[DisplayName("AccountDate")] 
		public DateTime  AccountDate { get; set; }

		[DisplayName("Source")] 
		public string  Source { get; set; }

		[DisplayName("DocumentType")] 
		public string  DocumentType { get; set; }

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("DocumentDate")] 
		public DateTime  DocumentDate { get; set; }

		[DisplayName("DebtorCode")] 
		public string  DebtorCode { get; set; }

		[DisplayName("CreditorCode")] 
		public string  CreditorCode { get; set; }

		[DisplayName("ChequeNo")] 
		public string  ChequeNo { get; set; }

		[DisplayName("BankInSlipNo")] 
		public string  BankInSlipNo { get; set; }

		[DisplayName("DateReconciled")] 
		public DateTime  DateReconciled { get; set; }

		[DisplayName("BankStatementPgNo")] 
		public Int16  BankStatementPgNo { get; set; }

		[DisplayName("CurrencyCode")] 
		public string  CurrencyCode { get; set; }

		[DisplayName("ExchangeRate")] 
		public decimal ExchangeRate { get; set; }

		[DisplayName("BaseAmount")] 
		public decimal BaseAmount { get; set; }

        [DisplayName("LocalAmount")]
        public decimal LocalAmount { get; set; }

        [DisplayName("CreditAmount")]
        public decimal CreditAmount { get; set; }

        [DisplayName("DebitAmount")]
        public decimal DebitAmount { get; set; }

        [DisplayName("Remark")] 
		public string  Remark { get; set; }

		[DisplayName("BankStatementTotalPgNo")] 
		public Int16  BankStatementTotalPgNo { get; set; }

        public List<GLTransaction> GLTransactionDetails { get; set; }

        [DisplayName("Debit/Credit")]
        public string DebitCredit { get; set; }
        
        
        [DisplayName("Amount")]
        public decimal Amount { get; set; }


        [DisplayName("DetailRemark")]
        public string DetailRemark { get; set; }



        [DisplayName("Status")]
        public Boolean Status { get; set; }

        public IEnumerable<SelectListItem> AccountCodeList { get; set; }
        public IEnumerable<SelectListItem> DebitCreditList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> PeriodList { get; set; }

        [DisplayName("Year")]
        public Int16 Year { get; set; }

        [DisplayName("Period")]
        public Int16 Period{ get; set; }

	}
}




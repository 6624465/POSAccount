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
	public class CBReceipt: IContract
	{
		// Constructor 
		public CBReceipt() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("DocumentDate")] 
		public DateTime  DocumentDate { get; set; }

        [DisplayName("BranchID")]
        public Int16 BranchID { get; set; }

		[DisplayName("ReceiptType")] 
		public string  ReceiptType { get; set; }

		[DisplayName("BankCode")] 
		public string  BankCode { get; set; }

		[DisplayName("DebtorCode")] 
		public string  DebtorCode { get; set; }

		[DisplayName("PayerName")] 
		public string  PayerName { get; set; }

		[DisplayName("BankAccount")] 
		public string  BankAccount { get; set; }

		[DisplayName("BankChargesAccount")] 
		public string  BankChargesAccount { get; set; }

		[DisplayName("DebtorAccount")] 
		public string  DebtorAccount { get; set; }

		[DisplayName("ChequeNo")] 
		public string  ChequeNo { get; set; }

		[DisplayName("ChequeDate")] 
		public DateTime  ChequeDate { get; set; }

		[DisplayName("CurrencyCode")] 
		public string  CurrencyCode { get; set; }

		[DisplayName("ExchangeRate")] 
		public decimal ExchangeRate { get; set; }

		[DisplayName("BaseReceiptAmount")] 
		public decimal BaseReceiptAmount { get; set; }

		[DisplayName("BaseApplyAmount")] 
		public decimal BaseApplyAmount { get; set; }

		[DisplayName("LocalReceiptAmount")] 
		public decimal LocalReceiptAmount { get; set; }

		[DisplayName("LocalApplyAmount")] 
		public decimal LocalApplyAmount { get; set; }

		[DisplayName("LocalBankChargesAmount")] 
		public decimal LocalBankChargesAmount { get; set; }

		[DisplayName("Remark")] 
		public string  Remark { get; set; }

		[DisplayName("AccountDate")] 
		public DateTime  AccountDate { get; set; }

		[DisplayName("Source")] 
		public string  Source { get; set; }

        [DisplayName("IsWHTax")]
        public bool IsWHTax { get; set; }

        [DisplayName("WHPercent")]
        public short WHPercent { get; set; }

        [DisplayName("WHAmount")]
        public decimal WHAmount { get; set; }

        [DisplayName("IsVAT")]
        public bool IsVAT { get; set; }

        [DisplayName("TaxAmount")]
        public decimal TaxAmount { get; set; }

        [DisplayName("TotalAmount")]
        public decimal TotalAmount { get; set; }


		[DisplayName("IsCancel")] 
		public bool  IsCancel { get; set; }

		[DisplayName("CanceledBy")] 
		public string  CanceledBy { get; set; }

		[DisplayName("CanceledOn")] 
		public DateTime  CanceledOn { get; set; }

		[DisplayName("CreatedBy")] 
		public string  CreatedBy { get; set; }

		[DisplayName("CreatedOn")] 
		public DateTime  CreatedOn { get; set; }

		[DisplayName("ModifiedBy")] 
		public string  ModifiedBy { get; set; }

		[DisplayName("ModifiedOn")] 
		public DateTime  ModifiedOn { get; set; }



        [DisplayName("PrintRemarks")]
        public string PrintRemarks { get; set; }

        public List<CBReceiptDetail> CBReceiptDetails { get; set; }

        public List<CBReceiptSetOffDetail> CBReceiptSetOffDetails { get; set; }

        public List<CBReceiptGlDetail> CBReceiptGLDetails { get; set; }

        public List<GLTransaction> GLTransactionDetails { get; set; }

        public IEnumerable<SelectListItem> ReceiptTypeList { get; set; }

        public IEnumerable<SelectListItem> DebtorList { get; set; }

        public IEnumerable<SelectListItem> BankCodeList { get; set; }

        public IEnumerable<SelectListItem> CurrencyCodeList { get; set; }

	}
}




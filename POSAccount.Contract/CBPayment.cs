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
	public class CBPayment: IContract
	{
		// Constructor 
		public CBPayment() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("DocumentDate")] 
		public DateTime  DocumentDate { get; set; }

        [DisplayName("BranchID")]
        public Int16 BranchID { get; set; }

		[DisplayName("PaymentType")] 
		public string  PaymentType { get; set; }

		[DisplayName("BankCode")] 
		public string  BankCode { get; set; }

		[DisplayName("CreditorCode")] 
		public string  CreditorCode { get; set; }

		[DisplayName("PayeeName")] 
		public string  PayeeName { get; set; }

		[DisplayName("BankAccount")] 
		public string  BankAccount { get; set; }

		[DisplayName("BankChargesAccount")] 
		public string  BankChargesAccount { get; set; }

		[DisplayName("CreditorAccount")] 
		public string  CreditorAccount { get; set; }

		[DisplayName("ChequeNo")] 
		public string  ChequeNo { get; set; }

		[DisplayName("ChequeDate")] 
		public DateTime  ChequeDate { get; set; }

		[DisplayName("CurrencyCode")] 
		public string  CurrencyCode { get; set; }

		[DisplayName("ExchangeRate")] 
		public decimal ExchangeRate { get; set; }

		[DisplayName("BasePaymentAmount")] 
		public decimal BasePaymentAmount { get; set; }

		[DisplayName("BaseApplyAmount")] 
		public decimal BaseApplyAmount { get; set; }

		[DisplayName("LocalPaymentAmount")] 
		public decimal LocalPaymentAmount { get; set; }

		[DisplayName("LocalApplyAmount")] 
		public decimal LocalApplyAmount { get; set; }

		[DisplayName("LocalDiscountAmount")] 
		public decimal LocalDiscountAmount { get; set; }

		[DisplayName("LocalBankChargesAmount")] 
		public decimal LocalBankChargesAmount { get; set; }

		[DisplayName("Remark")] 
		public string  Remark { get; set; }

		[DisplayName("AccountDate")] 
		public DateTime  AccountDate { get; set; }

		[DisplayName("Source")] 
		public string  Source { get; set; }

        [DisplayName("IsWHTax")]
        public bool IsWHTax  { get; set; }

        [DisplayName("WHPercent")]
        public short WHPercent { get; set; }

        [DisplayName("WHAmount")]
        public decimal WHAmount { get; set; }

        [DisplayName("IsVAT")]
        public bool IsVAT { get; set; }

        [DisplayName("TaxAmount")]
        public decimal TaxAmount { get; set; }

        [DisplayName("NetAmount")]
        public decimal NetAmount { get; set; }


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

        /* The below 2 list are for CB Payment (CREDITOR) */        
        public List<CBPaymentSetOffDetail> CBPaymentSetOffDetails { get; set; }
        public List<CBPaymentGlDetail> CBPaymentGLDetails { get; set; }


        /* The below 2 list are for CB Payment (OTHERS) */
        public List<CBPaymentDetail> CBPaymentDetails { get; set; }
        public List<GLTransaction> GLTransactionDetails { get; set; }


        public IEnumerable<SelectListItem> PaymentTypeList { get; set; }

        public IEnumerable<SelectListItem> CreditorList { get; set; }

        public IEnumerable<SelectListItem> BankCodeList { get; set; }

        public IEnumerable<SelectListItem> CurrencyCodeList { get; set; }

        

	}
}




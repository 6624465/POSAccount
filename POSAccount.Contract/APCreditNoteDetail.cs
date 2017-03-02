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
	public class APCreditNoteDetail: IContract
	{
		// Constructor 
		public APCreditNoteDetail() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("ItemNo")] 
		public Int16  ItemNo { get; set; }

		[DisplayName("AccountCode")] 
		public string  AccountCode { get; set; }

        [DisplayName("AccountCodeDescription")]
        public string AccountCodeDescription { get; set; }

		[DisplayName("ChargeCode")] 
		public string  ChargeCode { get; set; }

		[DisplayName("OrderNo")] 
		public string  OrderNo { get; set; }

		[DisplayName("CurrencyCode")] 
		public string  CurrencyCode { get; set; }

		[DisplayName("ExchangeRate")] 
		public decimal ExchangeRate { get; set; }

		[DisplayName("BaseAmount")] 
		public decimal BaseAmount { get; set; }

		[DisplayName("LocalAmount")] 
		public decimal LocalAmount { get; set; }

		[DisplayName("TaxAmount")] 
		public decimal TaxAmount { get; set; }

		[DisplayName("LocalAmountWithTax")] 
		public decimal LocalAmountWithTax { get; set; }

		[DisplayName("TaxCode")] 
		public string  TaxCode { get; set; }

		[DisplayName("Remark")] 
		public string  Remark { get; set; }

		[DisplayName("CreatedBy")] 
		public string  CreatedBy { get; set; }

		[DisplayName("CreatedOn")] 
		public DateTime  CreatedOn { get; set; }

		[DisplayName("ModifiedBy")] 
		public string  ModifiedBy { get; set; }

		[DisplayName("ModifiedOn")] 
		public DateTime  ModifiedOn { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }


        public IEnumerable<SelectListItem> AccountCodeList { get; set; }
        public IEnumerable<SelectListItem> CurrencyCodeList { get; set; }
	}
}




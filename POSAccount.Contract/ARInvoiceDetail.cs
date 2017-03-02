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
	public class ARInvoiceDetail: IContract
	{
		// Constructor 
		public ARInvoiceDetail() { }

		// Public Members 

        [DisplayName("DocumentNo")]
        public string DocumentNo { get; set; }

        [DisplayName("ItemNo")]
        public Int16 ItemNo { get; set; }

        [DisplayName("AccountCode")]
        public string AccountCode { get; set; }

        [DisplayName("AccountCodeDescription")]
        public string AccountCodeDescription { get; set; }


        [DisplayName("ChargeCode")]
        public string ChargeCode { get; set; }


        [DisplayName("ChargeCodeDescription")]
        public string ChargeCodeDescription { get; set; }

        
        [DisplayName("OrderNo")]
        public string OrderNo { get; set; }

        [DisplayName("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [DisplayName("ExchangeRate")]
        public decimal ExchangeRate { get; set; }

        [DisplayName("BaseAmount")]
        public decimal BaseAmount { get; set; }

        [DisplayName("LocalAmount")]
        public decimal LocalAmount { get; set; }

        [DisplayName("DiscountType")]
        public string DiscountType { get; set; }

        [DisplayName("Discount")]
        public decimal Discount { get; set; }

        [DisplayName("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [DisplayName("TaxAmount")]
        public decimal TaxAmount { get; set; }

        [DisplayName("LocalAmountWithTax")]
        public decimal LocalAmountWithTax { get; set; }

        [DisplayName("TaxCode")]
        public string TaxCode { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

        [DisplayName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [DisplayName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Status")]
        public Boolean Status { get; set; }

        public IEnumerable<SelectListItem> AccountCodeList { get; set; }
        public IEnumerable<SelectListItem> CurrencyCodeList { get; set; }
        public IEnumerable<SelectListItem> DiscountTypeList { get; set; }


	}
}




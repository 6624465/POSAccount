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
	public class CBReceiptDetail: IContract
	{
		// Constructor 
		public CBReceiptDetail() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("ItemNo")] 
		public Int16  ItemNo { get; set; }

		[DisplayName("AccountCode")] 
		public string  AccountCode { get; set; }

		[DisplayName("Remark")] 
		public string  Remark { get; set; }

		[DisplayName("CurrencyCode")] 
		public string  CurrencyCode { get; set; }

		[DisplayName("ExchangeRate")] 
		public decimal ExchangeRate { get; set; }

		[DisplayName("BaseAmount")] 
		public decimal BaseAmount { get; set; }

		[DisplayName("BaseApplyAmount")] 
		public decimal BaseApplyAmount { get; set; }

		[DisplayName("LocalAmount")] 
		public decimal LocalAmount { get; set; }

		[DisplayName("LocalApplyAmount")] 
		public decimal LocalApplyAmount { get; set; }

		[DisplayName("MatchDocumentType")] 
		public string  MatchDocumentType { get; set; }

		[DisplayName("MatchDocumentNo")] 
		public string  MatchDocumentNo { get; set; }

		[DisplayName("MatchDocumentDate")] 
		public DateTime  MatchDocumentDate { get; set; }

		[DisplayName("CreditTermInDays")] 
		public Int16  CreditTermInDays { get; set; }

		[DisplayName("SetOffDate")] 
		public DateTime  SetOffDate { get; set; }

		[DisplayName("DebtorCode")] 
		public string  DebtorCode { get; set; }

        public IEnumerable<SelectListItem> CurrencyList { get; set; }

        public IEnumerable<SelectListItem> AccountCodeList { get; set; }


        public IEnumerable<SelectListItem> ChargeCodeList { get; set; }
    }
}




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
	public class APTransaction: IContract
	{
		// Constructor 
		public APTransaction() { }

		// Public Members 

		[DisplayName("CreditorCode")] 
		public string  CreditorCode { get; set; }

		[DisplayName("MatchDocumentType")] 
		public string  MatchDocumentType { get; set; }

		[DisplayName("MatchDocumentNo")] 
		public string  MatchDocumentNo { get; set; }

		[DisplayName("DocumentType")] 
		public string  DocumentType { get; set; }

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("DocumentTypeSequence")] 
		public Int16  DocumentTypeSequence { get; set; }

		[DisplayName("DocumentDate")] 
		public DateTime  DocumentDate { get; set; }

		[DisplayName("CreditTermInDays")] 
		public Int16  CreditTermInDays { get; set; }

		[DisplayName("CurrencyCode")] 
		public string  CurrencyCode { get; set; }

		[DisplayName("ExchangeRate")] 
		public decimal ExchangeRate { get; set; }

		[DisplayName("BaseAmount")] 
		public decimal BaseAmount { get; set; }

		[DisplayName("LocalAmount")] 
		public decimal LocalAmount { get; set; }

		[DisplayName("AccountDate")] 
		public DateTime  AccountDate { get; set; }


	}
}




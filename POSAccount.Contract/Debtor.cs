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
	public class Debtor: IContract
	{
		// Constructor 
		public Debtor() { }

		// Public Members 

		[DisplayName("DebtorCode")] 
		public string DebtorCode { get; set; }

		[DisplayName("DebtorName")] 
		public string DebtorName { get; set; }

		[DisplayName("RegistrationNo")] 
		public string  RegistrationNo { get; set; }

		[DisplayName("VATNo")] 
		public string  VATNo { get; set; }

		[DisplayName("CreditTerm")] 
		public string  CreditTerm { get; set; }

		[DisplayName("DebtorAccount")] 
		public string  DebtorAccount { get; set; }

		[DisplayName("AddressID")] 
		public string  AddressID { get; set; }

		[DisplayName("CreatedBy")] 
		public string  CreatedBy { get; set; }

		[DisplayName("CreatedOn")] 
		public DateTime  CreatedOn { get; set; }

		[DisplayName("ModifiedBy")] 
		public string  ModifiedBy { get; set; }

		[DisplayName("ModifiedOn")] 
		public DateTime  ModifiedOn { get; set; }

		[DisplayName("Status")] 
		public bool  Status { get; set; }

        [DisplayName("IsAutoSendInvoice")]
        public bool IsAutoSendInvoice { get; set; }

        

        [DisplayName("Payment Type")]
        public string PaymentType { get; set; }

        [DisplayName("DebtorAccount Description")]
        public string DebtorAccountDescription { get; set; }

        public Address DebtorAddress { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }

        public IEnumerable<SelectListItem> DebtorAccountList { get; set; }

        public IEnumerable<SelectListItem> PaymentTypeList { get; set; }

	}
}




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
	public class OrderDetails: IContract
	{
		// Constructor 
		public OrderDetails() { }

		// Public Members 

		[DisplayName("DocumentNo")] 
		public string  DocumentNo { get; set; }

		[DisplayName("ItemNo")] 
		public Int16  ItemNo { get; set; }

		[DisplayName("Package")] 
		public string ChargeCode { get; set; }

        [DisplayName("Charge Description")]
        public string ChargeDescription { get; set; }

		[DisplayName("CandidateName")] 
		public string CandidateName { get; set; }

		[DisplayName("ContactNo")] 
		public string ContactNo { get; set; }

		[DisplayName("Position")] 
		public string Position { get; set; }

		[DisplayName("JoiningDate")] 
		public DateTime  JoiningDate { get; set; }

		[DisplayName("EmailID")] 
		public string EmailID { get; set; }

		[DisplayName("JobStatus")] 
		public Int16  JobStatus { get; set; }

        [DisplayName("Salary")]
        public decimal Salary { get; set; }

        [DisplayName("SellRate")]
        public decimal SellRate { get; set; }

        [DisplayName("Quantity")]
        public long Quantity { get; set; }

        [DisplayName("SellPrice")]
        public decimal SellPrice { get; set; }

        [DisplayName("DiscountType")]
        public string DiscountType { get; set; }

        [DisplayName("Discount")]
        public decimal Discount  { get; set; }

        [DisplayName("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [DisplayName("TaxAmount")]
        public decimal TaxAmount { get; set; }

        [DisplayName("TotalAmountWithTax")]
        public decimal TotalAmountWithTax { get; set; }


		[DisplayName("Status")] 
		public bool  Status { get; set; }

		[DisplayName("CreatedBy")] 
		public string  CreatedBy { get; set; }

		[DisplayName("CreatedOn")] 
		public DateTime  CreatedOn { get; set; }

		[DisplayName("ModifiedBy")] 
		public string  ModifiedBy { get; set; }

		[DisplayName("ModifiedOn")] 
		public DateTime  ModifiedOn { get; set; }


        public IEnumerable<SelectListItem> ChargeCodeList { get; set; }

        public IEnumerable<SelectListItem> DiscountTypeList { get; set; }


	}
}




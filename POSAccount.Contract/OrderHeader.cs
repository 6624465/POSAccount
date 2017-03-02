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
    public class OrderHeader : IContract
    {
        // Constructor 
        public OrderHeader() { }

        // Public Members 

        [DisplayName("DocumentNo")]
        public string DocumentNo { get; set; }

        [DisplayName("DocumentDate")]
        public DateTime DocumentDate { get; set; }

        [DisplayName("CustomerCode")]
        public string CustomerCode { get; set; }

        [DisplayName("CustomerName")]
        public string CustomerName { get; set; }


        [DisplayName("BranchID")]
        public Int16 BranchID { get; set; }

        [DisplayName("OrderType")]
        public string OrderType { get; set; }

        [DisplayName("PaymentType")]
        public string PaymentType { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("IsWHTax")]
        public bool IsWHTax { get; set; }
        
        [DisplayName("IsVAT")]
        public bool IsVAT { get; set; }

        [DisplayName("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [DisplayName("DiscountAmount")]
        public decimal DiscountAmount { get; set; }

        [DisplayName("PaymentAmount")]
        public decimal PaymentAmount { get; set; }

        [DisplayName("TaxAmount")]
        public decimal TaxAmount { get; set; }

        [DisplayName("WithHoldingAmount")]
        public decimal WithHoldingAmount { get; set; }

        [DisplayName("NetAmount")]
        public decimal NetAmount { get; set; }


        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

        [DisplayName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [DisplayName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("IsApproved")]
        public bool IsApproved { get; set; }

        [DisplayName("ApprovedBy")]
        public string ApprovedBy { get; set; }

        [DisplayName("ApprovedOn")]
        public DateTime ApprovedOn { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }

        public IEnumerable<SelectListItem> CustomerList { get; set; }

        public IEnumerable<SelectListItem> OrderTypeList { get; set; }
        public IEnumerable<SelectListItem> PaymentTypeList { get; set; }
    }
}




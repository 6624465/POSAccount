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
    public class Quotation : IContract
    {
        // Constructor 
        public Quotation() { }

        // Public Members 

        [DisplayName("Quotation No.")]
        public string QuotationNo { get; set; }

        [DisplayName("BranchID")]
        public Int16 BranchID { get; set; }

        [DisplayName("Quotation Date")]
        public DateTime QuotationDate { get; set; }

        [DisplayName("Effective Date")]
        public DateTime EffectiveDate { get; set; }

        [DisplayName("Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [DisplayName("Customer Code")]
        public string CustomerCode { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [DisplayName("Sales Man")]
        public string SalesMan { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified On")]
        public DateTime ModifiedOn { get; set; }


        public List<QuotationItem> QuotationItems { get; set; }

        public IEnumerable<SelectListItem> CustomerList { get; set; }

    }
}
